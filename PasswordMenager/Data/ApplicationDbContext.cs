using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PasswordMenager.Models;
using PasswordMenager.Models.VModels;

namespace PasswordMenager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<ClientPassword> clientPasswords { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}