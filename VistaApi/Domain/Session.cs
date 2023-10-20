using System.ComponentModel.DataAnnotations;

namespace VistaApi.Domain
{
    public class Session
    {
        public int SessionId { get; set; }

        public int TrainerId { get; set; }

        [Required]
        public DateTime SessionDate { get; set; }

        [MaxLength(40)]
        public string? BookingReference { get; set; } // Allow nulls

        public Trainer? Trainer { get; set; }

    }
}
