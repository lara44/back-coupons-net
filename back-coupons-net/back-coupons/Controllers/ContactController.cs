
using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Exceptions;
using back_coupons.Repositories.Interfaces;
using back_coupons.UnitsOfWork.Implementations;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_coupons.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    public class ContactController : GenericController<Contact>
    {
        private readonly IContactRepository _contactRepository;
        public ContactController(IGenericUnitOfWork<Contact> unit, IContactRepository contactRepository) : base(unit)
        {
            _contactRepository = contactRepository;
        }


        [HttpGet("full")]
        public override async Task<IActionResult> GetAsyncFull()
        {
            var action = await _contactRepository.GetAsyncFull();
            if (action.Successfully)
            {
                return Ok(new { data = action.Result });
            }
            return BadRequest();
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _contactRepository.GetAsync(pagination);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result, totalPages = response.TotalPage });
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _contactRepository.GetAsync(id);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return NotFound();
        }
    }
}
