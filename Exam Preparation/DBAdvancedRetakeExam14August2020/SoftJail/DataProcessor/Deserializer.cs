namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportDepartmentDto[] departmentDtos = JsonConvert.DeserializeObject<ImportDepartmentDto[]>(jsonString);

            HashSet<Department> validDepartments = new HashSet<Department>();

            foreach (ImportDepartmentDto departmentDto in departmentDtos)
            {
                if (!IsValid(departmentDto) || !departmentDto.Cells.All(IsValid) || !departmentDto.Cells.Any())
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                var department = new Department
                {
                    Name = departmentDto.Name,
                    Cells = departmentDto.Cells.Select(x => new Cell
                    {
                        CellNumber = x.CellNumber,
                        HasWindow = x.HasWindow
                    })
                        .ToList()
                };

                validDepartments.Add(department);

                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }
            context.Departments.AddRange(validDepartments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportPrisonerDto[] prisonerDtos = JsonConvert.DeserializeObject<ImportPrisonerDto[]>(jsonString);

            HashSet<Prisoner> validPrisoners = new HashSet<Prisoner>();

            foreach (ImportPrisonerDto prisonerDto in prisonerDtos)
            {
                if (!IsValid(prisonerDto) || !prisonerDto.Mails.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var isValidReleasedDate = DateTime.TryParseExact(prisonerDto.ReleaseDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releasedDate);

                var incarcerationDate = DateTime.ParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture);

                var prisoner = new Prisoner
                {
                    FullName = prisonerDto.FullName,
                    Nickname = prisonerDto.Nickname,
                    Age = prisonerDto.Age,
                    Bail = prisonerDto.Bail,
                    CellId = prisonerDto.CellId,
                    ReleaseDate = isValidReleasedDate ? (DateTime?)releasedDate : null,
                    IncarcerationDate = incarcerationDate,
                    Mails = prisonerDto.Mails.Select(m => new Mail
                    {
                        Sender = m.Sender,
                        Address = m.Address,
                        Description = m.Description
                    })
                        .ToList()
                };
                validPrisoners.Add(prisoner);
                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.Prisoners.AddRange(validPrisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Officers");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportOfficerPrisonerDto[]), xmlRoot);

            using StringReader sr = new StringReader(xmlString);

            ImportOfficerPrisonerDto[] officerDtos = (ImportOfficerPrisonerDto[])xmlSerializer.Deserialize(sr);

            HashSet<Officer> officers = new HashSet<Officer>();

            foreach (ImportOfficerPrisonerDto officerDto in officerDtos)
            {
                if(!IsValid(officerDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var officer = new Officer
                {
                    FullName = officerDto.Name,
                    Salary = officerDto.Money,
                    Position = Enum.Parse<Position>(officerDto.Position),
                    Weapon = Enum.Parse<Weapon>(officerDto.Weapon),
                    DepartmentId = officerDto.DepartmentId,
                    OfficerPrisoners = officerDto.Prisoners.Select(x => new OfficerPrisoner
                    {
                        PrisonerId = x.Id
                    })
                        .ToList()
                };
                officers.Add(officer);
                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
            }
            context.Officers.AddRange(officers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}