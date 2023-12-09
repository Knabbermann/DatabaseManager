using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseManager.Web.Pages
{
    public class CustomPageModel<T> : PageModel
    {
        public override string ToString()
        {
            var fullName = typeof(T).ToString().Replace($".{typeof(T).Name}", "");
            var index = fullName.LastIndexOf('.');
            return index < 0 ? fullName : fullName[(index + 1)..];
        }

        protected static bool GetRandomBool()
        {
            return new Random().Next(0, 2) == 1;
        }

        protected static int GetRandomInt()
        {
            return new Random().Next(1, 10000);
        }

        protected static int GetRandomInt(int min, int max)
        {
            return new Random().Next(min, max);
        }

        protected static decimal GetRandomDecimal()
        {
            var random = new Random();
            var next = random.NextDouble() * (9999.99 - 0.01) + 0.01;
            return Math.Round((decimal)next, 2);
        }
        protected static string GetRandomMailSuffix()
        {
            var random = new Random();
            var mailSuffix = new List<string>() { "@outlook.com", "@googlemail.com","@aol.com", "@web.de", "@gmx.de"};
            return mailSuffix[random.Next(0, mailSuffix.Count - 1)];
        }

        protected static DateTime GetRandomDateTime()
        {
            var random = new Random();
            var minDate = DateTime.MinValue;
            var range = DateTime.Now - minDate;
            var randomTimeSpan = new TimeSpan((long)(random.NextDouble() * range.Ticks));
            return minDate + randomTimeSpan;
        }

        protected static string GetRandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var stringChars = new char[new Random().Next(5, 11)];
            var random = new Random();
            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new string(stringChars);
        }

        protected static string GetRandomFromList(string fileName)
        {
            var lines = System.IO.File.ReadAllLines(@$"C:\Users\jiris\source\repos\DatabaseManager\DatabaseManager.Utilities\{fileName}.txt");
            var random = new Random();
            var randomLine = lines[random.Next(lines.Length)];
            return randomLine;
        }
    }
}
