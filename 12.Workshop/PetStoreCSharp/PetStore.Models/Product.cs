using PetStore.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public class Product
    {
        public Product()
        {
            this.Id = Guid.NewGuid().ToString();
            this.AvailableStores = new HashSet<Store>();
            this.Sales = new HashSet<ProductSale>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(ProductValidationConstans.NAME_MAX_LENGTH)]
        public string Name { get; set; }
        [MaxLength(ProductValidationConstans.DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }
        [Required]
        [MaxLength(ProductValidationConstans.URL_MAX_LENGTH)]
        public string ImageURL { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        [Required]
        [MaxLength(ProductValidationConstans.DISTRIBUTOR_NAME_MAX_LENGTH)]
        public string DistributorName { get; set; }

        public bool InStock => this.Quantity > 0;

        [ForeignKey(nameof(ProductType))]
        public int ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; }

        public virtual ICollection<Store> AvailableStores { get; set; }

        public virtual ICollection<ProductSale> Sales { get; set; }
    }
}
