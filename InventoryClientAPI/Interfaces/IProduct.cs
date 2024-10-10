using InventoryClientAPI.Models;

namespace InventoryClientAPI.Interfaces {
	public interface IProduct {
		// Obtener todos los productos
		Task<List<Product>> GetAllProductsAsync();

		// Obtener un producto por su ID
		Task<Product> GetProductByIdAsync(Guid productId);

		// Añadir un nuevo producto al inventario
		Task AddProductAsync(Product product);
		Task AddProductsBulk(List<Product> product);

		// Actualizar un producto existente
		Task UpdateProductAsync(Product product);

		// Eliminar un producto por su ID
		Task RemoveProductAsync(Guid productId);

		Task MarkAsDefectiveAsync(Guid productId);
	}
}
