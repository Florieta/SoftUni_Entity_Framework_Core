﻿namespace VaporStore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using VaporStore.Data.Models.Enums;

    public class Card
    {
        public Card()
        {
            this.Purchases = new HashSet<Purchase>();
        }
        public int Id { get; set; }

        [Required]
        //[MaxLength(19)]
        public string Number { get; set; }

        [Required]
        //[MaxLength(3)]
        public string Cvc { get; set; }

        public CardType Type { get; set; }//by default Required

        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

    }
}