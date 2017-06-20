using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Plants.API.Models;

namespace Plants.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Plant> Plants { get; set; }
        public DbSet<FavoritePlant> FavoritePlants { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PlantCategory> PlantCategories { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<FavoritePlant>().HasKey(fp => new { fp.PlantId, fp.UserId });
            builder.Entity<PlantCategory>().HasKey(pc => new { pc.PlantId, pc.CategoryId });
        }
    }
}
