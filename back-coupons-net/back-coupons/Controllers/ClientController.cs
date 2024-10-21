using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_coupons.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientController : GenericController<Client>
    {
        private readonly IClientUnitOfWork _clientUnitOfWork;
        public ClientController(IGenericUnitOfWork<Client> unit, IClientUnitOfWork clientUnitOfWork) : base(unit)
        {
            _clientUnitOfWork = clientUnitOfWork;
        }

        [HttpGet("full")]
        public override async Task<IActionResult> GetAsyncFull()
        {
            var action = await _clientUnitOfWork.GetAsyncFull();
            if (action.Successfully)
            {
                return Ok(new { data = action.Result });
            }
            return BadRequest();
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _clientUnitOfWork.GetAsync(pagination);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _clientUnitOfWork.GetAsync(id);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return NotFound();
        }
    }
}
