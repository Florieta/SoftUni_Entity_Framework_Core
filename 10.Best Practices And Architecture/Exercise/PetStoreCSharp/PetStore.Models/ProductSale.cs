﻿using PetStore.Common;
using PetStore.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public class ProductSale
    {
        [Required]
        [ForeignKey(nameof(Client))]
        public string ClientId { get; set; }

        public virtual Client Client { get; set; }

        [Required]
        [ForeignKey(nameof(Product))]
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        public DateTime DateTime { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public PaymentType PaymentType { get; set; }

        [Required]
        [ForeignKey(nameof(Address))]
        public string AddressId { get; set; }

        public virtual Address Address { get; set; }

        [ForeignKey(nameof(CardInfo))]
        public string CardId { get; set; }

        public virtual CardInfo CardInfo { get; set; }
        [MaxLength(ProductSaleValidationConstans.Bill_INFO_MAX_LENGTH)]
        public string BillInfo { get; set; }
    }
}
