using Azure;
using back_coupons.DTOs;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_coupons.Controllers
{
    public class GenericController<T> : Controller where T : class
    {
        private readonly IGenericUnitOfWork<T> _unitOfWork;

        public GenericController(IGenericUnitOfWork<T> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("full")]
        public virtual async Task<IActionResult> GetAsyncFull()
        {
            var action = await _unitOfWork.GetAsyncFull();
            if (action.Successfully)
            {
                return Ok(new { data = action.Result });
            }
            return BadRequest();
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _unitOfWork.GetAsync(pagination);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public virtual async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _unitOfWork.GetTotalPagesAsync(pagination);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetAsync(int id)
        {
            var response = await _unitOfWork.GetAsync(id);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return NotFound();
        }

        [HttpPost]
        public virtual async Task<IActionResult> PostAsync(T model)
        {
            var response = await _unitOfWork.AddAsync(model);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return BadRequest(response.Message);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> PutAsync(int id, T updatedModel)
        {
            var response = await _unitOfWork.UpdateAsync(id, updatedModel);
            if (response.Successfully)
            {
                return Ok(new { data = response.Result });
            }
            return BadRequest(response.Message);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteAsync(int id)
        {
            var action = await _unitOfWork.DeleteAsync(id);
            if (action.Successfully)
            {
                return NoContent();
            }
            return BadRequest(action.Message);
        }
    }

}
