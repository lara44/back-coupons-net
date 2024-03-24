
using back_coupons.Exceptions;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_coupons.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserUnitOfWork _userUnitOfWork;

        public UserController(IUserUnitOfWork userUnitOfWork)
        {
            _userUnitOfWork = userUnitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userUnitOfWork.GetAllAsync();
                return Ok(new { users = users });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la lista de usuarios: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userUnitOfWork.GetUserByIdAsync(id);
                return Ok(new { user = user });
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el usuario: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> createUser([FromBody] Entities.User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (user == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _userUnitOfWork.CreateUserAsync(user);
                if (response != null)
                {
                    return Ok(new { user = user });
                }

                return BadRequest("No se pudo guardar en el sistema");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error guardar: {ex.Message}");
            }
        }

    }
}
