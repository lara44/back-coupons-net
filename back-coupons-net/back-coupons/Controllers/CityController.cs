using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_coupons.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CityController : GenericController<City>
    {
        private readonly ICityUnitOfWork _cityUnitOfWork;
        public CityController(IGenericUnitOfWork<City> unit, ICityUnitOfWork cityUnitOfWork) : base(unit)
        {
            _cityUnitOfWork = cityUnitOfWork;
        }

        [HttpGet("full")]
        public override async Task<IActionResult> GetAsyncFull()
        {
            var response = await _cityUnitOfWork.GetAsyncFull();
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return BadRequest();
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _cityUnitOfWork.GetAsync(pagination);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result, totalPages = response.TotalPage  });
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _cityUnitOfWork.GetAsync(id);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return NotFound();
        }

        [HttpGet("state/{state}/cities")]
        public async Task<IActionResult> GetCitiesByStatesAsync(int state)
        {
            var response = await _cityUnitOfWork.GetCitiesByStatesAsync(state);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return NotFound();
        }
    }
}
