using System.ComponentModel.DataAnnotations;

namespace VistaApi.Domain
{
    public class Trainer
    {
        public int TrainerId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string Location { get; set; } = null!;

        public List<TrainerCategory>? TrainerCategories { get; set; }

        public List<Session>? Sessions { get; set; }

    }
}
