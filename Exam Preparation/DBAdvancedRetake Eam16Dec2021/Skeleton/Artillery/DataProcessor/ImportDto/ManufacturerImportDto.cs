using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Manufacturer")]
    public class ManufacturerImportDto
    {
        [XmlElement("ManufacturerName")]
        [Required]
        [MaxLength(4)]
        [MinLength(40)]
        public string ManufacturerName { get; set; }

        [XmlElement("Founded")]
        [Required]
        [MaxLength(10)]
        [MinLength(100)]
        public string Founded { get; set; }
    }
}
