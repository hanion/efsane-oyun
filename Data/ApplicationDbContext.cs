using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using efsane_oyun.Models;
using Microsoft.AspNetCore.Identity;

namespace efsane_oyun.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<efsane_oyun.Models.Games> Games { get; set; } = default!;
        public DbSet<efsane_oyun.Models.Comments> Comments { get; set; } = default!;
        public DbSet<efsane_oyun.Models.Ratings> Ratings { get; set; } = default!;
        public DbSet<efsane_oyun.Models.Tags> Tags { get; set; } = default!;
        public DbSet<efsane_oyun.Models.GameTags> GameTags { get; set; } = default!;
    }
}
