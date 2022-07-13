using PetStore.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public class Breed
    {
        public Breed()
        {
            this.Pets = new HashSet<Pet>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(BreedValidationConstans.NAME_MAX_LENGTH)]
        public string Name { get; set; }

        public virtual ICollection<Pet> Pets { get; set; }
    }
}
