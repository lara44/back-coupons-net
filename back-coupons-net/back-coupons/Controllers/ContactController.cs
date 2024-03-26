
using back_coupons.Exceptions;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_coupons.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    public class ContactController : ControllerBase
    {
        private readonly IContactUnitOfWork _contactUnitOfWork;

        public ContactController(IContactUnitOfWork contactUnitOfWork)
        {
            _contactUnitOfWork = contactUnitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContact()
        {
            try
            {
                var contacts = await _contactUnitOfWork.GetAllAsync();
                return Ok(new { contacts = contacts });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la lista de contactos: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactById(int id)
        {
            try
            {
                var contact = await _contactUnitOfWork.GetContactByIdAsync(id);
                return Ok(new { contact = contact });
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener datos del contacto: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> createContact([FromBody] Entities.Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (contact == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _contactUnitOfWork.CreateContactAsync(contact);
                if (response != null)
                {
                    return Ok(new { contact = contact });
                }

                return BadRequest("No se pudo guardar el registo de contacto en el sistema");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> updateContact([FromBody] Entities.Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (contact == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _contactUnitOfWork.UpdateContactAsync(contact);
                if (response != null)
                {
                    return Ok(new { contact = contact });
                }

                return BadRequest("No se pudo actualizar el contacto en el sistema");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
