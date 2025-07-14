// Data/ApplicationDbContext.cs

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyECommerceSite.Models; // არ დაგავიწყდეს მოდელების დაკავშირება

namespace MyECommerceSite.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // ეს ხაზი ეუბნება EF Core-ს, რომ ჩვენ გვინდა "Products" სახელის ცხრილი,
        // რომელიც შეესაბამება ჩვენს Product მოდელს.
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ეს ხაზი აუცილებლად უნდა იყოს პირველი
            base.OnModelCreating(modelBuilder);

            // --- დამატებული კოდი ---
            // ვუთითებთ, რომ Price ველს ბაზაში ჰქონდეს decimal(18, 2) ტიპი
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");
            // --- კოდის დასასრული ---

            // პროდუქტების საწყისი მონაცემებით შევსება (Seeding)
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = ".NET-ის მაისური (ბაზიდან)",
                    Description = "მაღალი ხარისხის ბამბის მაისური დეველოპერებისთვის.",
                    Price = 45.50m,
                    ImageUrl = "https://www.booska-p.com/wp-content/uploads/2024/06/Messi-Argentine.jpg"
                },
                new Product
                {
                    Id = 2,
                    Name = "C# ყავის ჭიქა (ბაზიდან)",
                    Description = "დაიწყე დილა კოდთან ერთად.",
                    Price = 25.00m,
                    ImageUrl = "https://ohmyfacts.com/wp-content/uploads/2024/04/15-ronaldo-facts-fans-should-know-1714300205.jpg"
                },
                new Product
                {
                    Id = 3,
                    Name = "Visual Studio სტიკერები (ბაზიდან)",
                    Description = "დაამშვენე შენი ლეპტოპი.",
                    Price = 15.99m,
                    ImageUrl = "https://via.placeholder.com/400"
                }
            );
        }
    }
}