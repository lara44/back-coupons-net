
using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_coupons.Controllers
{

    [ApiController]
    [Route("api/products")]
    public class ProductController : GenericController<Product>
    {
        private readonly IProductUnitOfWork _productUnitOfWork;
        public ProductController(IGenericUnitOfWork<Product> unit, IProductUnitOfWork productUnitOfWork) : base(unit)
        {
            _productUnitOfWork = productUnitOfWork;
        }

        [HttpGet("GetProductsByCompany")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllFullAsync(int CompanyId)
        {
            var response = await _productUnitOfWork.GetAllFullAsync(CompanyId);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return BadRequest();
        }

        [HttpGet("GetProductsByCompanyPagination")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _productUnitOfWork.GetAsync(pagination);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result, totalPages = response.TotalPage });
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _productUnitOfWork.GetAsync(id);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return NotFound();
        }
    }
}
