using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Theatre.Common;
using Theatre.Data.Models.Enums;

namespace Theatre.Data.Models
{
    public class Play
    {
        public Play()
        {
            this.Tickets = new HashSet<Ticket>();
            this.Casts = new HashSet<Cast>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(GlobalConstants.PLAY_TITLE_MAX_LENGTH)]
        public string Title { get; set; }

        public TimeSpan Duration { get; set; }

        public float Rating { get; set; }

        public Genre Genre { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PLAY_DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PLAY_SCREENWRITER_MAX_LENGTH)]
        public string Screenwriter { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public ICollection<Cast> Casts { get; set; }
    }
}
