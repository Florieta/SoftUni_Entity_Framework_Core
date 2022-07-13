using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using PetStore.Common;
using PetStore.Models.Enumerations;

namespace PetStore.Models
{
    public class Pet
    {
        public Pet()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Pets = new HashSet<Pet>();
        }

        [Key]
        public string Id { get; set; }

        [MaxLength(PetValidationConstans.PET_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [MaxLength(PetValidationConstans.DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }

        public byte Age { get; set; }
        public Gender Gender { get; set; }
        [Required]
        [MaxLength(PetValidationConstans.URL_MAX_LENGTH)]
        public string ImageURL { get; set; }
        public bool IsSold { get; set; }

        public decimal Price { get; set; }

        [ForeignKey(nameof(PetType))]
        public int PetTypeId { get; set; }
        public virtual PetType PetType { get; set; }

        [Required]
        [ForeignKey(nameof(Breed))]
        public int BreedId { get; set; }
        public virtual Breed Breed { get; set; }

        [ForeignKey(nameof(Client))]
        public string ClientId { get; set; }
        public virtual Client Client { get; set; }

        [Required]
        [ForeignKey(nameof(Store))]
        public string StoreId { get; set; }

        public virtual Store Store { get; set; }

        public virtual ICollection<Pet> Pets { get; set; }
        
        [ForeignKey(nameof(PetReservation))]
        public string ReservationId { get; set; }

        public PetReservation PetReservation { get; set; }
    }
}