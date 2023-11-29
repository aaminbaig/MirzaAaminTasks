using System.ComponentModel.DataAnnotations;

namespace Task2.Models
{
    public class User
    {
        [Key] 
        public int userid { get; set; }
        [Required]
        public string username { get; set; }
        [Required] 
        public string Role { get; set; }
        [Required] 
        public byte[] passwordsalt { get; set; }
        [Required] 
        public byte[] passwordhash { get; set; }
    }
}
