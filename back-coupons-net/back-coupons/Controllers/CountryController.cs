using back_coupons.Entities;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_coupons.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : GenericController<Country>
    {
        private readonly ICountryUnitOfWork _countryUnitOfWork;
        public CountryController(IGenericUnitOfWork<Country> unitOfWork, ICountryUnitOfWork countryUnitOfWork) : base(unitOfWork)
        {
            _countryUnitOfWork = countryUnitOfWork;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            var action = await _countryUnitOfWork.GetAsync();
            if (action.Successfully)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var action = await _countryUnitOfWork.GetAsync(id);
            if (action.Successfully)
            {
                return Ok(action.Result);
            }
            return NotFound();
        }
    }
}
