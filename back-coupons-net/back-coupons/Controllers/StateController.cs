using back_coupons.Entities;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_coupons.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StateController : GenericController<State>
    {
        private readonly IStateUnitOfWork _stateUnitOfWork;
        public StateController(IGenericUnitOfWork<State> unitOfWork, IStateUnitOfWork stateUnitOfWork) : base(unitOfWork)
        {
            _stateUnitOfWork = stateUnitOfWork;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            var action = await _stateUnitOfWork.GetAsync();
            if (action.Successfully)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var action = await _stateUnitOfWork.GetAsync(id);
            if (action.Successfully)
            {
                return Ok(action.Result);
            }
            return NotFound();
        }
    }
}
