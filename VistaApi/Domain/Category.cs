using System.ComponentModel.DataAnnotations;

namespace VistaApi.Domain
{
    public class Category
    {
        [Key]  // Since name (CategoryCode) does not include "Id",
               // we have to use an annotation (could also specify
               // this using FluitAPI) 
        [MaxLength(15)]
        public string CategoryCode { get; set; } = null!;

        [Required]
        [MaxLength(30)]
        public string CategoryName { get; set; } = null!;

        public List<TrainerCategory>? TrainerCategories { get; set; }

    }
}
