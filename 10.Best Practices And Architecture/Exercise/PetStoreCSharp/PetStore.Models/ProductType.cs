using PetStore.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public class ProductType
    {
        public ProductType()
        {
            this.Products = new HashSet<Product>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ProductTypeValidationConstans.NAME_MAX_LENGTH)]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
