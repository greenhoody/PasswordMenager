using System.ComponentModel.DataAnnotations;

namespace PasswordMenager.Models
{
    public class ClientPassword
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        public byte[] Password { get; set; }
        [Required, Url]
        public string URI { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
