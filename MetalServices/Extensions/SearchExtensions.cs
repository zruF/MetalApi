using MetalModels.Contracts;
using MetalModels.Models;
using Shared.Dtos.Requests;

namespace MetalServices.Extensions
{
    public static class SearchExtensions
    {
        public static IQueryable<IEntity> SetQueryFilter(this IQueryable<IEntity> query, SearchFilterRequest request)
        {
            if(request.Name != null)
            {
                query = query.Where(q => q.Name.Contains(request.Name));
            }

            return query;
        }

        public static string GetParentGenre(this string genre)
        {
            var suffixList = new List<string> { "Deathcore", "Metalcore", "Hardcore", "Crossover", "Slam", "Electronic", "Ambient", "Rock 'n' Roll", "Roll", "Djent" };

            var genreList = genre.Split(' ').ToList();

            if (!genreList.Contains("Rock") && !genreList.Contains("Metal"))
            {
                foreach(var suffix in suffixList)
                {
                    if (genreList.Contains(suffix))
                    {
                        return suffix;
                    }
                }

                return genre;
            }
            else
            {
                string suffix = genreList.Contains("Rock") ? "Rock" : "Metal";

                var ind = genreList.IndexOf(suffix);

                if(ind == 0)
                {
                    return genreList[ind];
                }
                else
                {
                    return $"{genreList[ind - 1]} {genreList[ind]}";
                }
            }
        }

        public static DateTime ParseDate(this string date)
        {
            if(date.Split('/').First().Length == 1)
            {
                date = "0" + date;
            }
            return DateTime.ParseExact(date, "MM/dd/yyyy", null);
        }
    }
}
