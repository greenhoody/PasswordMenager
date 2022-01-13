using Microsoft.EntityFrameworkCore;

namespace PasswordMenager.Models.VModels
{
    [Keyless]
    public class ClientPasswordVM
    {
        public string? URI { get; set; }
        public string? Password { get; set; }
    }
}
