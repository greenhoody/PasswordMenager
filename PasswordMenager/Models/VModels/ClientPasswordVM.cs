using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PasswordMenager.Models.VModels
{
    [Keyless]
    public class ClientPasswordVM
    {
        [Required, Url]
        public string? URI { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
