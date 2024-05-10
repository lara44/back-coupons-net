using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_coupons.Controllers
{
    [ApiController]
    [Route("api/states")]
    public class StateController : GenericController<State>
    {
        private readonly IStateUnitOfWork _stateUnitOfWork;
        public StateController(IGenericUnitOfWork<State> unitOfWork, IStateUnitOfWork stateUnitOfWork) : base(unitOfWork)
        {
            _stateUnitOfWork = stateUnitOfWork;
        }

        [HttpGet("full")]
        public override async Task<IActionResult> GetAsyncFull()
        {
            var response = await _stateUnitOfWork.GetAsyncFull();
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return BadRequest();
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _stateUnitOfWork.GetAsync(pagination);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _stateUnitOfWork.GetAsync(id);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return NotFound();
        }

        [HttpGet("countries/{country}/states")]
        public async Task<IActionResult> GetStatesByCountryListAsync(int country)
        {
            var response = await _stateUnitOfWork.GetStatesByCountryListAsync(country);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return NotFound();
        }
    }
}
