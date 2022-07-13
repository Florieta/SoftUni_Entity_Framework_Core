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
    public class Client
    {
        public Client()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }
        [Required]
        [MaxLength(ClientValidationConstans.NAME_MAX_LENGTH)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(ClientValidationConstans.NAME_MAX_LENGTH)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(ClientValidationConstans.EMAIL_MAX_LENGTH)]
        public string Email { get; set; }

        [Required]
        [MaxLength(ClientValidationConstans.PASSWORD_MAX_LENGTH)]
        public string Password { get; set; }

        [Required]
        [MaxLength(ClientValidationConstans.PHONE_NUMBER_MAX_LENGTH)]
        public string PhoneNumber { get; set; }

        [ForeignKey(nameof(Address))]
        public string AddressId { get; set; }

        public virtual Address Address { get; set; }

        [ForeignKey(nameof(Card))]
        public string CardId { get; set; }

        public virtual CardInfo Card { get; set; }
    }
}
