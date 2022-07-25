using Artillery.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artillery.Data.Models
{
    public class Gun
    {
        public Gun()
        {
            CountriesGuns = new HashSet<CountryGun>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey(nameof(Manufacturer))]
        public int ManufacturerId { get; set; }

        public Manufacturer Manufacturer { get; set; }
        [Required]
        [MaxLength(1350000)]
        public int GunWeight { get; set; }
        [Required]
        [MaxLength(35)]
        public double BarrelLength { get; set; }

        public int? NumberBuild { get; set; }
        [Required]
        [MaxLength(100000)]
        public int Range { get; set; }
        [Required]
        public GunType GunType { get; set; }
        [ForeignKey(nameof(Shell))]
        public int ShellId { get; set; }

        public Shell Shell { get; set; }

        public ICollection<CountryGun> CountriesGuns { get; set; }
    }
}
