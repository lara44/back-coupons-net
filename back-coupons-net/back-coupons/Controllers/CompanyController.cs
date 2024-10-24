using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_coupons.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompanyController : GenericController<Company>
    {
        private readonly ICompanyUnitOfWork _companyUnitOfWork;
        public CompanyController(IGenericUnitOfWork<Company> unit, ICompanyUnitOfWork companyUnitOfWork) : base(unit)
        {
            _companyUnitOfWork = companyUnitOfWork;
        }

        [HttpGet("GetCompanyByUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCompanyByUser(string userId)
        {
            var action = await _companyUnitOfWork.GetCompanyByUserAsync(userId);
            if (action.Successfully)
            {
                return Ok(new { data = action.Result });
            }
            return BadRequest();
        }

        [HttpGet("full")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> GetAsyncFull()
        {
            var action = await _companyUnitOfWork.GetAsyncFull();
            if (action.Successfully)
            {
                return Ok(new { data = action.Result });
            }
            return BadRequest();
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _companyUnitOfWork.GetAsync(pagination);
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
            var response = await _companyUnitOfWork.GetAsync(id);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return NotFound();
        }
    }
}
