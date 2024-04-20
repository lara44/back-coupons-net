using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.UnitsOfWork.Implementations;
using back_coupons.UnitsOfWork.Interfaces;
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

        [HttpGet("full")]
        public override async Task<IActionResult> GetAsync()
        {
            var action = await _companyUnitOfWork.GetAsync();
            if (action.Successfully)
            {
                return Ok(new { data = action.Result });
            }
            return BadRequest();
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _companyUnitOfWork.GetAsync(pagination);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
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
