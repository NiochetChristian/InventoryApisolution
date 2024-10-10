using InventoryClientAPI.Interfaces;
using InventoryClientAPI.Models;
using InventoryClientAPI.Models.ModelContext;
using InventoryClientAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace InventoryClientAPI.Services
{
    public class ProductService : IProduct
    {

        private AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todos los productos
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        // Obtener un producto por su ID
        public async Task<Product> GetProductByIdAsync(Guid productId)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId) ?? new();
        }

        // Añadir un nuevo producto al inventario
        public async Task AddProductAsync(Product product)
        {
            product.ProductId = Guid.NewGuid();
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task AddProductsBulk(List<Product> products)
        {
            if (!Tools.ValidateStatusEnum(products)) throw new("Uno o mas productos no contienen un tipo de estado valido.");

            foreach (var product in products)
            {
                product.ProductId = Guid.NewGuid();
                product.DateAdded = DateTime.Now;
                product.LastUpdated = DateTime.Now;
            }

            await _context.Products.AddRangeAsync(products);

            await _context.SaveChangesAsync();

        }

        // Actualizar un producto existente
        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.ProductId);
            if (existingProduct == null)
            {
                throw new Exception("Producto no encontrado");
            }

            existingProduct.ProductName = product.ProductName;
            existingProduct.ProductionType = product.ProductionType;
            existingProduct.ProductStatus = product.ProductStatus;
            existingProduct.LastUpdated = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        // Eliminar un producto por su ID
        public async Task RemoveProductAsync(Guid productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAsDefectiveAsync(Guid productId)
        {

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product != null)
            {
                product.ProductStatus = "Defectuoso";
                await _context.SaveChangesAsync();
            }
        }
    }
}
