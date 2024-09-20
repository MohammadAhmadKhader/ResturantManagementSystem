using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResturantManagementSystem.Models;
using System.Reflection.Emit;

namespace ResturantManagementSystem.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public DbSet<Chef> Chefs { get; set; }
		public DbSet<Food> Foods { get; set; }
		public DbSet<Ingredient> Ingredients { get; set; }
		public DbSet<Order> Orders { get; set; }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
        protected override void OnModelCreating(ModelBuilder builder)
        {
			base.OnModelCreating(builder);

			builder.Entity<OrderFood>().HasKey(of => new { of.OrderId, of.FoodId });
			builder.Entity<OrderFood>()
				.HasOne(of => of.Food)
				.WithMany(f => f.OrderFoods)
				.HasForeignKey(of => of.FoodId);
			builder.Entity<OrderFood>()
				.HasOne(of => of.Order)
				.WithMany(o => o.OrderFoods)
				.HasForeignKey(of => of.OrderId);

            builder.Entity<FoodIngredient>().HasKey(fi => new { fi.FoodId, fi.IngredientId });
            builder.Entity<FoodIngredient>()
                .HasOne(fi => fi.Ingredient)
                .WithMany(i => i.FoodIngredients)
                .HasForeignKey(fi => fi.IngredientId);
            builder.Entity<FoodIngredient>()
                .HasOne(fi => fi.Food)
                .WithMany(o => o.FoodIngredients)
                .HasForeignKey(fi => fi.FoodId);

            builder.Entity<Chef>()
				.HasIndex(c => c.UserId)
				.IsUnique();

            builder.Entity<Ingredient>()
                .HasIndex(c => c.Name)
                .IsUnique();

            builder.Entity<Food>()
                .HasIndex(c => c.Name)
                .IsUnique();
        }
    }
}
