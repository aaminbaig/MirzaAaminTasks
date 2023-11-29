using System.ComponentModel.DataAnnotations;

namespace Task2.Models
{
    public class login
    {
        [Required]
        public string username { get; set; } = string.Empty;
        [Required]
        public string password { get; set; } = string.Empty;

    }
}
