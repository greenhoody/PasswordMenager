using System.ComponentModel.DataAnnotations;

namespace PasswordMenager.Models.VModels
{
    public class ClientPasswordIndexVM
    {
        [Key]
        public int Id { get; set; }
        public string? URI { get; set; }
        public string? Password { get; set; }
    }
}
