using InventoryClientAPI.Interfaces;
using InventoryClientAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryClientAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private IProduct _productService;

        public ProductController(IProduct productService)
        {
            _productService = productService;
        }

        // GET: api/product
        // Obtener la lista de todos los productos, categorizados por estado
        [HttpGet("GetProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/product/{id}
        // Obtener un producto por su ID único
        [HttpGet("GetProduct/{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: api/product
        // Registrar un nuevo producto en el stock
        [Authorize]
        [HttpPost("AddProduct")]
        public async Task<ActionResult> AddProduct([FromBody] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.AddProductAsync(product);
                    return Ok($"Producto creado (id - {product.ProductId})");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest(ModelState);
        }

        // POST: api/product/bulk
        // Registrar productos de manera masiva
        [Authorize]
        [HttpPost("AddProductsBulk")]
        public async Task<ActionResult> AddProductsBulk([FromBody] List<Product> products)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.AddProductsBulk(products);
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest(ModelState);
        }

        // PUT: api/product/{id}
        // Actualizar un producto existente en el stock
        [Authorize]
        [HttpPut("UpdateProduct")]
        public async Task<ActionResult> UpdateProduct([FromBody] Product product)
        {
            try
            {
                await _productService.UpdateProductAsync(product);
                return Ok("Producto actualizado");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PATCH: api/product/mark-defective/{id}
        // Marcar un producto como defectuoso
        [Authorize]
        [HttpPatch("MarkProductAsDefective/{id}")]
        public async Task<ActionResult> MarkProductAsDefective(Guid id)
        {
            try
            {
                await _productService.MarkAsDefectiveAsync(id);
                return Ok("Producto marcado como defectuoso"); // Retorna "NoContent" (HTTP 204) si la operación es exitosa.
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/product/{id}
        // Eliminar un producto del stock
        [Authorize]
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            try
            {
                await _productService.RemoveProductAsync(id); // Elimina el producto del sistema.
                return Ok("Producto borrado correctamente");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
