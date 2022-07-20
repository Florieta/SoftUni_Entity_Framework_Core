namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Books");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportBookDto[]), xmlRoot);

            using StringReader sr = new StringReader(xmlString);

            ImportBookDto[] bookDtos = (ImportBookDto[])xmlSerializer.Deserialize(sr);

            HashSet<Book> books = new HashSet<Book>();

            foreach (ImportBookDto bookDto in bookDtos)
            {
                if(!IsValid(bookDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                
                bool isDateValid = DateTime.TryParseExact(bookDto.PublishedOn, "MM/dd/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime publishedOn);
                
                if(!isDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Book b = new Book()
                {
                    Name = bookDto.Name,
                    Genre = (Genre)bookDto.Genre,
                    Price = bookDto.Price,
                    PublishedOn = publishedOn
                };

                books.Add(b);
                sb.AppendLine(String.Format(SuccessfullyImportedBook, b.Name, b.Price));
            }

            context.Books.AddRange(books);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportAuthorDto[] authorDtos = JsonConvert.DeserializeObject<ImportAuthorDto[]>(jsonString);

            List<Author> authors = new List<Author>();

            foreach (ImportAuthorDto authorDto in authorDtos)
            {
                if (!IsValid(authorDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (authors.Any(a => a.Email == authorDto.Email))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Author author = new Author()
                {
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName,
                    Email = authorDto.Email,
                    Phone = authorDto.Phone
                };

                foreach (ImportAuthorBookDto bookDto in authorDto.Books)
                {
                    if (!bookDto.BookId.HasValue)
                    {
                        continue;
                    }
                    Book book = context
                        .Books
                        .FirstOrDefault(b => b.Id == bookDto.BookId);

                    if (book == null)
                    {
                        continue;
                    }
                    author.AuthorsBooks.Add(new AuthorBook
                    {
                        Author = author,
                        Book = book
                    });
                }

                if (author.AuthorsBooks.Count == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                authors.Add(author);
                sb.AppendLine(String.Format(SuccessfullyImportedAuthor, (author.FirstName + ' ' + author.LastName),
                    author.AuthorsBooks.Count));
            }
            context.Authors.AddRange(authors);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}