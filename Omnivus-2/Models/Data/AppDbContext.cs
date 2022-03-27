using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Omnivus_2.Models.Entites;

namespace Omnivus_2.Models.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected AppDbContext()
        {
        }




        public DbSet<ProfileEntity> Profiles { get; set; }

    }
}
