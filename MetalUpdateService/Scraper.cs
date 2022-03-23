using HtmlAgilityPack;
using MetalModels;
using MetalModels.Models;
using MetalModels.Types;
using MetalUpdateService.Contracts;
using MetalUpdateService.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace MetalUpdateService
{
    public class Scraper
    {
        private IConfigHandler _config;
        private MetalDbContext _dbContext;

        private string[] albumTypes;

        public Scraper(IConfigHandler config, MetalDbContext dbContext)
        {
            _config = config;
            _dbContext = dbContext;
        }

        public async Task ProcessAsync()
        {
            await InitializeAsync();

            var (updated, createdUrl, query) = (_config.GetConfig("UpdateUrl"), _config.GetConfig("CreatedUrl"), _config.GetConfig("UpdateQuery"));

            // Get Added
            for (var currentStack = 400; currentStack < 1000; currentStack += 200)
            {
                var url = $"{updated}{string.Format(query, currentStack, 10000)}";
                var rawCode = await new HtmlWeb().LoadFromWebAsync(url);
                var nodes = rawCode.DocumentNode.SelectNodes("//a[@href]").Where((n,i) => i%3 == 0).ToList();
                await ScrapeBandsAsync(nodes);
            }

            // Get Updated
            for (var currentStack = 0; currentStack <= 400; currentStack += 200)
            {
                var url = $"{createdUrl}{string.Format(query, currentStack, 10000)}";
                var rawCode = await new HtmlWeb().LoadFromWebAsync(url);
                var nodes = rawCode.DocumentNode.SelectNodes("//a[@href]").Where((n, i) => i % 3 == 0).ToList();
                await ScrapeBandsAsync(nodes);
            }
        }

        private async Task ScrapeBandsAsync(List<HtmlNode> bandEntries)
        {
            var nodes = new List<HtmlNode>();

            var bands = new List<Band>();

            for (var i = 0; i < bandEntries.Count; i++)
            {
                var bandUrl = bandEntries[i].GetAttributeValue("href", "NoUrlFound").Replace("\\", "").Replace("\"", "");
                var url = bandEntries[i].Attributes.FirstOrDefault().Value.Replace("\\", "").Replace("\"", "");
                var bandName = bandEntries[i].InnerText;
                var bandId = url.Split("/").Last();
                var albumNodes = await GetAlbumNodesAsync(bandId);

                var existingBand = await _dbContext.Bands
                    .Include(b => b.Albums)
                    .FirstOrDefaultAsync(b => b.ShortId == bandId);
                var bandGuid = existingBand?.BandId ?? Guid.NewGuid();

                if (albumNodes is null)
                {
                    continue;
                }

                var albums = await ScrapeAlbumsAsync(bandGuid, albumNodes);

                if (existingBand != null)
                {
                    if (albums.Count() > existingBand.Albums.Count())
                    {
                        albums = albums.Where(a => !existingBand.Albums.Any(b => b.Name != a.Name));
                        await _dbContext.Albums.AddRangeAsync(albums);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    var site = await new HtmlWeb().LoadFromWebAsync(bandUrl);
                    var bandInfos = site.DocumentNode.SelectNodes("//div[@id='band_stats']//dd");
                    var imgUrl = site.DocumentNode.SelectSingleNode("//a[@id='photo']")?.GetAttributeValue("href", "N/A");
                    var genres = await ScrapeGenresAsync(bandInfos[4].InnerText);

                    var band = new Band
                    {
                        BandId = bandGuid,
                        ShortId = bandId,
                        Country = bandInfos[1].InnerText,
                        IsActive = bandInfos[2].InnerText.Equals("Active"),
                        FoundingYear = int.Parse(bandInfos[3].InnerText == "N/A" ? "0" : bandInfos[3].InnerText),
                        Name = bandName,
                        ImgUrl = imgUrl,
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
                }

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
                var albumUrl = childs[0].ChildNodes[0].GetAttributeValue("href", null);
                string imgUrl = null;
                var site = await new HtmlWeb().LoadFromWebAsync(albumUrl);
                if (albumUrl != null)
                {
                    imgUrl = site.DocumentNode.SelectSingleNode("//a[@id='cover']")?.GetAttributeValue("href", "N/A");
                }
                var albumId = Guid.NewGuid();

                var albumInfos = site.DocumentNode.SelectNodes("//div[@id='album_info']//dd");

                albums.Add(new Album
                {
                    AlbumId = albumId,
                    BandId = bandId,
                    Name = albumName,
                    AlbumType = (AlbumType)Enum.Parse(typeof(AlbumType), childs[1].InnerText.Replace("-", "").Replace(" ", "").Trim()),
                    ReleaseDate = albumInfos[1].InnerText.ParseDate(),
                    ImgUrl = imgUrl,
                }); ;
            }

            return albums;
        }

        private async Task<IEnumerable<Genre>> ScrapeGenresAsync(string genreString)
        {
            var genres = new List<Genre>();

            var genreList = genreString.Split(";");

            var suffixList = new List<string> { "Rock", "Metal", "core", "Crossover", "Slam", "Electronic", "Ambient", "Roll", "Djent" };

            foreach (var genre in genreList)
            {
                var genresSplitted = Regex.Replace(genre, @"\(.*\)", "").Trim().Split(new char[] { '/', ',' });
                for (var i = 0; i < genresSplitted.Length; i++)
                {
                    var genreName = genresSplitted[i].Trim();
                    if (suffixList.All(s => !genresSplitted[i].Contains(s)))
                    {
                        //get next Suffix
                        var suffix = genresSplitted
                            .Skip(i + 1)
                            .Take(genresSplitted.Length)
                            .FirstOrDefault(g => new List<string> { "Metal", "Rock" }.Any(s => g.Contains(s)))?
                            .Split(' ')
                            .Last();

                        genreName = (genresSplitted[i] + " " + suffix ?? "").Trim();
                    }

                    var existingGenre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == genreName);

                    if (existingGenre == null)
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

        private async Task<IEnumerable<HtmlNode>> GetAlbumNodesAsync(string bandId)
        {
            var albumUrl = string.Format(_config.GetConfig("AlbumUrl"), bandId);
            var rawCode = await new HtmlWeb().LoadFromWebAsync(albumUrl);
            var tableNodes = rawCode.DocumentNode.SelectNodes("//table[@class='display discog']//tbody[1]//tr");
            if (tableNodes.Count == 1 && tableNodes.FirstOrDefault().InnerText.Contains("Nothing entered yet"))
            {
                return null;
            }
            var albumNodes = tableNodes
                .Where(n => albumTypes.Contains(n.SelectNodes("td")[1]?.InnerText.Replace("-", "").Replace(" ", "").Trim()))
                .ToList();

            return albumNodes;
        }

        private async Task InitializeAsync()
        {
            albumTypes = Enum.GetNames(typeof(AlbumType));
        }
    }
}