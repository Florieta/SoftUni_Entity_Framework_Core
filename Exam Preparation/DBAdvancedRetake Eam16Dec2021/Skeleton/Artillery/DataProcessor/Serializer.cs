
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shellsToExport = context.Shells
                .Where(s => s.ShellWeight > shellWeight)
                .OrderBy(s => s.ShellWeight)
                .Select(s => new
                {
                    ShellWeight = s.ShellWeight,
                    Caliber = s.Caliber,
                    Guns = s.Guns
                    .Where(g => (int)g.GunType == 3)
                    .OrderByDescending(g => g.GunWeight)
                    .Select(g => new
                    {
                        GunType = g.GunType.ToString(),
                        GunWeight = g.GunWeight,
                        BarrelLength = g.BarrelLength,
                        Range = g.Range > 3000 ? "Long-range" : "Regular range"
                    })
                    .ToArray()
                })
                .ToArray();

            string result = JsonConvert.SerializeObject(shellsToExport, Formatting.Indented);

            return result;

        }

    public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            

            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Guns");
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            XmlSerializer serializer = new XmlSerializer(typeof(GunExportDto[]), xmlRoot);

            using StringWriter sw = new StringWriter(sb);

            var gunsToExport = context.Guns
                .Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                .OrderBy(g => g.BarrelLength)
                .Select(g => new GunExportDto()
                {
                    Manufacturer = g.Manufacturer.ManufacturerName,
                    GunType = g.GunType.ToString(),
                    BarrelLength = g.BarrelLength,
                    GunWeight = g.GunWeight,
                    Range = g.Range,
                    Countries = g.CountriesGuns
                    .Where(c => c.Country.ArmySize > 4500000)
                    .OrderBy(c => c.Country.ArmySize)
                    .Select(s => new CountryExportDto
                    {
                        Country = s.Country.CountryName,
                        ArmySize = s.Country.ArmySize
                    })
                    .ToArray()
                })
                .ToArray();


            serializer.Serialize(sw, gunsToExport, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
