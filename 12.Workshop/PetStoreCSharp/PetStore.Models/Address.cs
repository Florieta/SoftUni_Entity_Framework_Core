using PetStore.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public class Address
    {
        public Address()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Clients = new HashSet<Client>();
            this.Stores = new HashSet<Store>();
            this.ProductSales = new HashSet<ProductSale>();
        }

        [Key]
        public string Id { get; set; }
        [Required]
        [MaxLength(AddressValidationConstans.TOWN_NAME_MAX_LENGTH)]
        public string TownName { get; set; }

        [Required]
        [MaxLength(AddressValidationConstans.ADDRESS_TEXT_MAX_LENGTH)]
        public string AddressText { get; set; }

        [MaxLength(AddressValidationConstans.NOTES_MAX_LENGTH)]
        public string Notes { get; set; }

        public virtual ICollection<Client> Clients { get; set; }

        public virtual ICollection<Store> Stores { get; set; }

        public virtual ICollection<ProductSale> ProductSales { get; set; }


    }
}
