using Microsoft.EntityFrameworkCore;

namespace InventoryClientAPI.Models.ModelContext {
	public class AppDbContext : DbContext {
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<Product> Products { get; set; } // Define el DbSet para la tabla Products

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Product>()
				.Property(p => p.ProductStatus)
				.HasConversion<string>(); // Almacena el enum como string en la base de datos

			base.OnModelCreating(modelBuilder);
		}
	}
}
