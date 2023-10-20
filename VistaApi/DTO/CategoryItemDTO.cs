using System.ComponentModel.DataAnnotations;
using VistaApi.Domain;

namespace VistaApi.DTO
{
    public class CategoryItemDTO
    {
            [Required]
            [MaxLength(15)]
            public string CategoryCode { get; set; } = null!;

            [Required]
            [MaxLength(30)]
            public string CategoryName { get; set; } = null!;

        }
    }
