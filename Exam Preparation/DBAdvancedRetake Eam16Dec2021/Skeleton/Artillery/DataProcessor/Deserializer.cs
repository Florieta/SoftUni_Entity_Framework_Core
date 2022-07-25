namespace Artillery.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage =
                "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Countries");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CountryImportDto[]), xmlRoot);

            using StringReader sr = new StringReader(xmlString);

            CountryImportDto[] countryDtos = (CountryImportDto[])xmlSerializer.Deserialize(sr);

            HashSet<Country> countries = new HashSet<Country>();

            foreach (var countryDto in countryDtos)
            {
                if (!IsValid(countryDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Country c = new Country()
                {
                    CountryName = countryDto.CountryName,
                    ArmySize = countryDto.ArmySize
                };

                countries.Add(c);
                sb.AppendLine(String.Format(SuccessfulImportCountry, c.CountryName, c.ArmySize));
            }

            context.Countries.AddRange(countries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Manufacturers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ManufacturerImportDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(xmlString);

            ManufacturerImportDto[] ManufacturerDtos = (ManufacturerImportDto[])xmlSerializer.Deserialize(stringReader);

            ICollection<Manufacturer> manufacturers = new HashSet<Manufacturer>();

            foreach (var dto in ManufacturerDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (manufacturers.Any(m => m.ManufacturerName == dto.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                string[] foundedDataArray = dto.Founded.Split(", ", StringSplitOptions.RemoveEmptyEntries).ToArray();

                if (foundedDataArray.Length < 2)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                string countryName = foundedDataArray[foundedDataArray.Length - 1];
                string townName = foundedDataArray[foundedDataArray.Length - 2];

                Manufacturer manufacturer = new Manufacturer()
                {
                    ManufacturerName = dto.ManufacturerName,
                    Founded = dto.Founded
                };

                manufacturers.Add(manufacturer);

                sb.AppendLine(string.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, $"{townName}, {countryName}"));
            }

            context.AddRange(manufacturers);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Shells");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ShellImportDto[]), xmlRoot);

            using StringReader sr = new StringReader(xmlString);

            ShellImportDto[] shellDtos = (ShellImportDto[])xmlSerializer.Deserialize(sr);

            HashSet<Shell> shells = new HashSet<Shell>();

            foreach (var shellDto in shellDtos)
            {
                if (!IsValid(shellDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Shell s = new Shell()
                {
                    ShellWeight = shellDto.ShellWeight,
                    Caliber = shellDto.Caliber
                };

                shells.Add(s);
                sb.AppendLine(String.Format(SuccessfulImportShell, s.Caliber, s.ShellWeight));
            }

            context.Shells.AddRange(shells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            GunImportDto[] gunDtos = JsonConvert.DeserializeObject<GunImportDto[]>(jsonString);

            HashSet<Gun> guns = new HashSet<Gun>();

            foreach (GunImportDto gunDto in gunDtos)
            {
                if (!IsValid(gunDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool hasGunType = Enum.TryParse<GunType>(gunDto.GunType, true, out GunType gunType);

                if (!hasGunType)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Gun g = new Gun()
                {
                    ManufacturerId = gunDto.ManufacturerId,
                    GunWeight = gunDto.GunWeight,
                    BarrelLength = gunDto.BarrelLength,
                    NumberBuild = gunDto.NumberBuild, 
                    Range = gunDto.Range,
                    GunType = gunType,
                    ShellId = gunDto.ShellId,

                };
                HashSet<CountryGun> countryGuns = new HashSet<CountryGun>();

                foreach (var countryId in gunDto.Countries)
                {
                    Country country = context.Countries.FirstOrDefault(c => c.Id == countryId.Id);

                    CountryGun countryGun = new CountryGun()
                    {
                        Gun = g,
                        Country = country
                    };

                    countryGuns.Add(countryGun);
                }

                g.CountriesGuns = countryGuns;

                guns.Add(g);

                sb.AppendLine(string.Format(SuccessfulImportGun, g.GunType, g.GunWeight, g.BarrelLength));
            }

            context.AddRange(guns);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
