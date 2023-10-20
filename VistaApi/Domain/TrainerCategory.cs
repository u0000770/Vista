using System.ComponentModel.DataAnnotations;

namespace VistaApi.Domain
{
    public class TrainerCategory
    {
        // Has a composite (compound) key that is defined in the TrainersDbConext

        [Required]
        public int TrainerId { get; set; }

        [Required]
        [MaxLength(15)]
        public string CategoryCode { get; set; } = null!;

        public Trainer Trainer { get; set; }

        public Category Category { get; set; }
        // See TrainersDbConext for Foreign Key (Fluent API) definition

    }
}
