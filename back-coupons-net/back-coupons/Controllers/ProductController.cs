
using back_coupons.Exceptions;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_coupons.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductUnitOfWork _productUnitOfWork;

        public ProductController(IProductUnitOfWork productUnitOfWork)
        {
            _productUnitOfWork = productUnitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productUnitOfWork.GetAllAsync();
                return Ok(new { products = products });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la lista de productos: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productUnitOfWork.GetByIdAsync(id);
                return Ok(new { product = product });
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el producto: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> createProduct([FromBody] Entities.Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (product == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _productUnitOfWork.CreateAsync(product);
                if (response != null)
                {
                    return Ok(new { product = product });
                }

                return BadRequest("No se pudo guardar en el sistema");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error guardar: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> updateProduct([FromBody] Entities.Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (product == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _productUnitOfWork.UpdateAsync(product);
                if (response != null)
                {
                    return Ok(new { product = product });
                }

                return BadRequest("No se pudo actualizar el producto en el sistema");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
