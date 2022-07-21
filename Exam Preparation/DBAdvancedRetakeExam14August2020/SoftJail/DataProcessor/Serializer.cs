namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
             var prisonersByCells = context
                .Prisoners
                .ToArray()
                .Where(x => ids.Contains(x.Id))
                .Select(e => new
                {
                    Id = e.Id,
                    Name = e.FullName,
                    CellNumber = e.Cell.CellNumber,
                    Officers = e.PrisonerOfficers.Select(x => new
                    {
                        OfficerName = x.Officer.FullName,
                        Department = x.Officer.Department.Name
                    })
                    .OrderBy(x => x.OfficerName)
                    .ToList(),
                    TotalOfficerSalary = decimal.Parse(e.PrisonerOfficers
                        .Sum(x => x.Officer.Salary)
                        .ToString("F2"))
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToList();

            string json = JsonConvert.SerializeObject(prisonersByCells, Formatting.Indented);

            return json;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Prisoners");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportPrisonerDto[]), xmlRoot);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);


            using StringWriter sw = new StringWriter(sb);

            var names = prisonersNames.Split(',', StringSplitOptions.RemoveEmptyEntries);

            var result = context.Prisoners
                .Where(x => names.Contains(x.FullName))
                .Select(x => new ExportPrisonerDto
                {
                    Id = x.Id,
                    Name = x.FullName,
                    IncarcerationDate = x.IncarcerationDate.ToString("yyyy-MM-dd"),
                    EncryptedMessages = x.Mails.Select(m => new
                            EncryptedMessageDto
                    {
                        Description = String.Join("", m.Description.Reverse())
                    })
                        .ToArray()
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToArray();

            xmlSerializer.Serialize(sw, result, namespaces);
            return sb.ToString().TrimEnd();
        }
    }
}