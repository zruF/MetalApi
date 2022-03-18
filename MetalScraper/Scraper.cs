using HtmlAgilityPack;
using MetalModels;
using MetalModels.Models;
using MetalModels.Types;
using MetalScraper.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace MetalScraper
{
    public class Scraper
    {
        private IConfigHandler _config;
        private MetalDbContext _dbContext;

        public Scraper(IConfigHandler config, MetalDbContext dbContext)
        {
            _config = config;
            _dbContext = dbContext;
        }

        public async Task ProcessAsync()
        {
            var stackSize = int.Parse(_config.GetConfig("EntryStackSize"));
            var maximumEntries = int.Parse(_config.GetConfig("MaximumEntries"));
            var threads = int.Parse(_config.GetConfig("Threads"));

            for (var currentStack = 0; currentStack < maximumEntries;)
            {
                var nodes = new List<HtmlNode>();
                var tasks = new List<Task>();
                for(var i = 0; i < threads; i++, currentStack+=stackSize)
                {
                    tasks.Add(Task.Run(async () => {
                        var url = $"{_config.GetConfig("BasicUrl")}{string.Format(_config.GetConfig("Query"), currentStack, maximumEntries)}";
                        var rawCode = await new HtmlWeb().LoadFromWebAsync(url);
                        lock(nodes)
                            nodes.AddRange(rawCode.DocumentNode.SelectNodes("//a[@href]").ToList());
                        Console.WriteLine("Scraping Web - " + nodes.Count);
                    }));
                }

                Task.WaitAll(tasks.ToArray());
                await ScrapeBandsAsync(nodes);
            }
        }

        private async Task ScrapeBandsAsync(List<HtmlNode> bandEntries)
        {
            var threads = int.Parse(_config.GetConfig("Threads"));

            var nodes = new List<HtmlNode>();

            var bands = new List<Band>();

            for (var i = 0; i <= bandEntries.Count; i++)
            {
                var bandUrl = bandEntries[i].GetAttributeValue("href", "NoUrlFound").Replace("\\", "").Replace("\"", "");
                var url = bandEntries[i].Attributes.FirstOrDefault().Value.Replace("\\","").Replace("\"", "");
                var bandId = long.Parse(url.Split("/").Last());
                var albumNodes = await GetAlbumNodesAsync(bandId);
                var bandGuid = Guid.NewGuid();
                var albums = await ScrapeAlbumsAsync(bandGuid, albumNodes);

                var existingBand = await _dbContext.Bands.FirstOrDefaultAsync(b => b.ShortId == bandId);

                if(existingBand != null)
                {
                    if(_dbContext.Albums.Where(a => a.BandId == existingBand.BandId).Count() != albums.Count())
                    {
                        continue;
                    }
                }
                    
                var bandInfos = (await new HtmlWeb().LoadFromWebAsync(bandUrl)).DocumentNode.SelectNodes("//div[@id='band_stats']//dd");
                var bandName = bandEntries[i].InnerText;
                    
                var genres = await ScrapeGenresAsync(bandInfos[4].InnerText);
                    
                var band = new Band
                {
                    BandId = bandGuid,
                    ShortId = bandId,
                    Country = bandInfos[1].InnerText,
                    IsActive = bandInfos[2].InnerText.Equals("Active"),
                    FoundingYear = int.Parse(bandInfos[3].InnerText == "N/A" ? "0" : bandInfos[3].InnerText),
                    Name = bandName,
                    Albums = albums
                };

                var bandGenres = genres.Select(g => new BandGenre
                {
                    BandId = band.BandId,
                    GenreId = g.GenreId,
                    Band = band,
                    Genre = g
                });

                await _dbContext.Bands.AddAsync(band);
                await _dbContext.BandGenres.AddRangeAsync(bandGenres);
                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task<IEnumerable<Album>> ScrapeAlbumsAsync(Guid bandId, IEnumerable<HtmlNode> albumEntries)
        {
            var albumList = albumEntries.ToList();

            var albums = new List<Album>();

            for (var i = 0; i < albumList.Count; i++)
            {
                var childs = albumList[i].SelectNodes("td");
                var albumName = childs[0].InnerText;
                var albumId = Guid.NewGuid();
                Enum.TryParse(typeof(AlbumType), childs[1].InnerText.Replace("-", "").Replace(" ", "").Trim(), out var albumType);

                albums.Add(new Album
                {
                    AlbumId = albumId,
                    BandId = bandId,
                    Name = albumName,
                    Songs = await ScrapeSongsAsync(albumId, childs[0].LastChild.GetAttributeValue("href", "UrlNotFound")),
                    AlbumType = albumType == null ? AlbumType.None : (AlbumType)albumType,
                    ReleaseYear = int.Parse(childs[2].InnerText == "N/A" ? "0" : childs[2].InnerText),
                });
            }

            return albums;
        }

        private async Task<IEnumerable<Song>> ScrapeSongsAsync(Guid albumId, string albumUrl)
        {
            var songs = new List<Song>();
            var songNodes = (await new HtmlWeb().LoadFromWebAsync(albumUrl)).DocumentNode.SelectNodes("//table[@class='display table_lyrics']//tr").ToList();
            
            foreach(var songNode in songNodes)
            {
                var childs = songNode.SelectNodes("td");
                if (childs.Count != 4)
                {
                    continue;
                }
                songs.Add(new Song
                {
                    SongId = Guid.NewGuid(),
                    AlbumId = albumId,
                    Length = childs[2].InnerText,
                    Name = childs[1].InnerText.Replace("\n", "")
                }); ;
            }

            return songs;
        }

        private async Task<IEnumerable<Genre>> ScrapeGenresAsync(string genreString)
        {
            var genres = new List<Genre>();

            var genreList = genreString.Split(";");

            var suffixList = new List<string>{ "Rock", "Metal", "core", "Crossover", "Slam", "Electronic", "Ambient", "Roll", "Djent"};

            foreach(var genre in genreList)
            {
                var genresSplitted = Regex.Replace(genre, @"\(.*\)", "").Trim().Split('/');
                for(var i = 0; i < genresSplitted.Length; i++)
                {
                    var genreName = genresSplitted[i];
                    if (suffixList.All(s => !genresSplitted[i].Contains(s)))
                    {
                        //get next Suffix
                        var suffix = genresSplitted
                            .Skip(i + 1)
                            .Take(genresSplitted.Length)
                            .FirstOrDefault(g => new List<string> { "Metal", "Rock" }.Any(s => g.Contains(s)))?
                            .Split(' ')
                            .Last();

                        genreName = (genresSplitted[i] + " " + suffix??"").Trim();
                    }

                    var existingGenre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == genreName);

                    if(existingGenre == null)
                    {
                        genres.Add(new Genre
                        {
                            Name = genreName,
                            GenreId = Guid.NewGuid()
                        });
                    }
                    else
                    {
                        genres.Add(existingGenre);
                    }
                }
            }

            return genres.Distinct();
        }

        private async Task<IEnumerable<HtmlNode>> GetAlbumNodesAsync(long bandId)
        {
            var albumUrl = string.Format(_config.GetConfig("AlbumUrl"), bandId);
            var rawCode = await new HtmlWeb().LoadFromWebAsync(albumUrl);
            var tableNodes = rawCode.DocumentNode.SelectNodes("//table[@class='display discog']//tbody[1]//tr").ToList();

            return tableNodes;
        }
    }
}
