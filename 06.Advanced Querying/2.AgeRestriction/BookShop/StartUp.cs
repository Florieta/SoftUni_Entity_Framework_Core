using System;
using System.Linq;
using System.Text;
using BookShop.Models.Enums;


namespace BookShop
{
    using Data;
    using Initializer;
    

    public class StartUp
    {
        public static object AgeResriction { get; private set; }

        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();

            AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            string[] bookTitles = context
                .Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            foreach (string title in bookTitles)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
