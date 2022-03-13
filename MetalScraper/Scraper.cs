using HtmlAgilityPack;
using MetalScraper.Contracts;
using MetalScraper.Models;
using MetalScraper.Types;

namespace MetalScraper
{
    public class Scraper
    {
        private IConfigHandler _config;

        public Scraper(IConfigHandler config)
        {
            _config = config;
        }

        public async Task ProcessAsync()
        {
            var stackSize = int.Parse(_config.GetConfig("EntryStackSize"));
            var maximumEntries = int.Parse(_config.GetConfig("MaximumEntries"));
            var threads = int.Parse(_config.GetConfig("Threads"));

            var nodes = new List<HtmlNode>();

            var bands = new List<Band>();

            for (var currentStack = 0; currentStack <= maximumEntries;)
            {
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
            }
            bands.AddRange(await ScrapeBands(nodes));
        }

        private async Task<IEnumerable<Band>> ScrapeBands(List<HtmlNode> bandEntries)
        {
            var threads = int.Parse(_config.GetConfig("Threads"));

            var nodes = new List<HtmlNode>();

            var bands = new List<Band>();

            for (var i = 0; i <= bandEntries.Count;)
            {
                for (var j = 0; j < threads; j++, i++)
                {
                    var bandUrl = bandEntries[i].GetAttributeValue("href", "NoUrlFound").Replace("\\", "").Replace("\"", "");
                    var bandName = bandEntries[i].InnerText;
                    var url = bandEntries[i].Attributes.FirstOrDefault().Value.Replace("\\","").Replace("\"", "");
                    var bandId = long.Parse(url.Split("/").Last());
                    var albumNodes = await GetAlbumNodesAsync(bandId);
                    var bandInfos = (await new HtmlWeb().LoadFromWebAsync(bandUrl)).DocumentNode.SelectNodes("//div[@id='band_stats']//dd");

                    Console.WriteLine("Scraping Band - " + bandName);
                    var albums = await ScrapeAlbumsAsync(bandId, albumNodes);
                    var genres = await ScrapeGenresAsync(bandInfos[4].InnerText);
                    lock (bands)
                        bands.Add(new Band
                        {
                            Id = Guid.NewGuid(),
                            Country = bandInfos[1].InnerText,
                            IsActive = bandInfos[2].InnerText.Equals("Active"),
                            FoundingYear = int.Parse(bandInfos[3].InnerText),
                            Name = bandName,
                            Genres = genres,
                            Albums = albums
                        });
                }
            }

            return bands;
        }

        private async Task<IEnumerable<Album>> ScrapeAlbumsAsync(long bandId, IEnumerable<HtmlNode> albumEntries)
        {
            var albumList = albumEntries.ToList();

            var albums = new List<Album>();

            for (var i = 0; i < albumList.Count; i++)
            {
                var childs = albumList[i].SelectNodes("td");
                var albumName = childs[0].InnerText;
                Console.WriteLine("--- Scraping Album - " + albumName);
                albums.Add(new Album
                {
                    Id = Guid.NewGuid(),
                    BandId = bandId,
                    Name = albumName,
                    Songs = await ScrapeSongsAsync(childs[0].LastChild.GetAttributeValue("href", "UrlNotFound")),
                    AlbumType = (AlbumType)Enum.Parse(typeof(AlbumType), childs[1].InnerText.Replace("-", "").Replace(" ", "").Trim()),
                    ReleaseYear = int.Parse(childs[2].InnerText == "N/A" ? "0" : childs[2].InnerText),
                });
            }

            return albums;
        }

        private async Task<IEnumerable<Song>> ScrapeSongsAsync(string albumUrl)
        {
            var x = albumUrl;
            return new List<Song>();
        }

        private async Task<IEnumerable<Genre>> ScrapeGenresAsync(string genre)
        {
            var genres = new List<Genre>();

            genre.Split(";");

            return genres;
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
