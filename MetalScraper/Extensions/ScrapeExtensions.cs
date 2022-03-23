using System.Text.RegularExpressions;

namespace MetalScraper.Extensions
{
    public static class ScrapeExtensions
    {
        public static string ParseDate(this string dateString)
        {
            if (!dateString.Contains(",") && !dateString.Contains(" "))
            {
                    return dateString;
            }

            var splitted = dateString.Split(" ");

            var month =
                (new List<string>
                { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" }
                .IndexOf(splitted.First()) + 1).ToString();

            var year = splitted.Last();

            var day = splitted.Length == 2 ? "01" : Regex.Replace(splitted[1], "[a-z,]*", "");

            if(day.Length == 1)
            {
                day = "0" + day;
            }

            if (month.Length == 1)
            {
                month = "0" + month;
            }

            return $"{month}/{day}/{year}";
        }
    }
}
