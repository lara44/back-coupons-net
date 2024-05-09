using Azure;
using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.UnitsOfWork.Implementations;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace back_coupons.Controllers
{
    [ApiController]
    [Route("api/countries")]
    public class CountryController : GenericController<Country>
    {
        private readonly ICountryUnitOfWork _countryUnitOfWork;
        public CountryController(IGenericUnitOfWork<Country> unitOfWork, ICountryUnitOfWork countryUnitOfWork) : base(unitOfWork)
        {
            _countryUnitOfWork = countryUnitOfWork;
        }

        [HttpGet("full")]
        public override async Task<IActionResult> GetAsyncFull()
        {
            var action = await _countryUnitOfWork.GetAsyncFull();
            if (action.Successfully)
            {
                return Ok(new { data = action.Result });
            }
            return BadRequest();
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _countryUnitOfWork.GetAsync(pagination);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _countryUnitOfWork.GetAsync(id);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return NotFound();
        }

        [HttpGet("GetCountryListAsync")]
        public async Task<IActionResult> GetCountryListAsync()
        {
            var action = await _countryUnitOfWork.GetCountryListAsync();
            if (action.Successfully)
            {
                return Ok(new { data = action.Result });
            }
            return BadRequest();
        }
    }
}
