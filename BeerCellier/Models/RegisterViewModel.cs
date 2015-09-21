using System.ComponentModel.DataAnnotations;

namespace BeerCellier.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}