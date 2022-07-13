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
    public class CardInfo
    {
        public CardInfo()
        {
            this.Id = Guid.NewGuid().ToString();
           
            this.ProductSales = new HashSet<ProductSale>();

        }

        [Key]
        public string Id { get; set; }
        [Required]
        [MaxLength(CardInfoValidationConstans.CARD_NUMBER_MAX_LENGTH)]
        public string Number { get; set; }
        [Required]
        [MaxLength(CardInfoValidationConstans.CARD_HOLDER_MAX_LENGTH)]
        public string HolderName { get; set; }
        [Required]
        [MaxLength(CardInfoValidationConstans.EXPIRATION_DATE_MAX_LENGTH)]
        public string ExpirationDate { get; set; }
        [Required]
        [MaxLength(CardInfoValidationConstans.CVC_MAX_LENGTH)]
        public string CVC { get; set; }

        [ForeignKey(nameof(Owner))]
        public string ClientId { get; set; }
        public virtual Client Owner { get; set; }

       

        public virtual ICollection<ProductSale> ProductSales { get; set; }




    }
}
