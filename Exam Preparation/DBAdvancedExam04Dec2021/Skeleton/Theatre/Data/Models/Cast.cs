using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Theatre.Common;

namespace Theatre.Data.Models
{
    public class Cast
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(GlobalConstants.CAST_FULLNAME_MAX_LENGTH)]
        public string FullName { get; set; }

        public bool IsMainCharacter { get; set; }

        [Required]
        public string PhoneNumber  { get; set; }

        [ForeignKey(nameof(Play))]
        public int PlayId { get; set; }

        public Play Play { get; set; }
    }
}
