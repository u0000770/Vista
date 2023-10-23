using Humanizer;
using System.ComponentModel.DataAnnotations;
using VistaApi.Domain;

namespace VistaApi.DTO
{
    public class TrainerItemDTO
    {

        public int TrainerId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string Location { get; set; } = null!;

    }

    public class TrainerDTO
    {

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string Location { get; set; } = null!;

    }

    public class TrainerCategoryDTO
    {

        public int TrainerId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string Location { get; set; } = null!;

        public List<DTO.CategoryItemDTO> Categories { get; set; }

        static public TrainerCategoryDTO buildDTO(Trainer trainer)
        {
            List<DTO.CategoryItemDTO> categories = new List<CategoryItemDTO>();
            TrainerCategoryDTO dto = new TrainerCategoryDTO();
            categories = trainer.TrainerCategories.Select(c => new CategoryItemDTO
            {
                CategoryCode = c.CategoryCode,
                CategoryName = c.Category.CategoryName
            }).ToList();
            if (categories.Count > 0)
            {
                dto.TrainerId = trainer.TrainerId;
                dto.Name = trainer.Name;
                dto.Location = trainer.Location;
                dto.Categories = categories;
                return dto;
            }
            return null;
        }
    }

    public class TrainerSessionDTO
    {

        public int TrainerId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string Location { get; set; } = null!;

        public List<DTO.SessionBookingDTO> Sessions { get; set; }

    }


    public class TrainerCategoryEditModel
    {
        public int TrainerId { get; set; }
        public List<string> Categories { get; set; }
    }

    public class SessionBookingDTO
    {
        [Required]
        public int SessionId { get; set; }

        [Required]
        public int TrainerId { get; set; }

        [Required]
        public DateTime SessionDate { get; set; }

        [MaxLength(40)]
        public string? BookingReference { get; set; } // Allow nulls

    }

}
