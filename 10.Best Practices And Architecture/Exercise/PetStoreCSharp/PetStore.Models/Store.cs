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
    public class Store
    {
        public Store()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Products = new HashSet<Product>();
            this.Pets = new HashSet<Pet>();
            this.Services = new HashSet<Service>();
           
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(StoreValidationConstans.NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [MaxLength(StoreValidationConstans.DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }
        [Required]
        [MaxLength(StoreValidationConstans.WORK_TIME_MAX_LENGTH)]
        public string WorkTime { get; set; }

        [Required]
        [MaxLength(StoreValidationConstans.EMAIL_MAX_LENGTH)]
        public string Email { get; set; }

        [Required]
        [MaxLength(StoreValidationConstans.PHONE_NUMBER_MAX_LENGTH)]
        public string PhoneNumber { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Pet> Pets { get; set; }

        public virtual ICollection<Service> Services { get; set; }


        [Required]
        [ForeignKey(nameof(Address))]
        public string AddressId { get; set; }

        public virtual Address Address { get; set; }
    }
}
