
using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Exceptions;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_coupons.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    public class ContactController : GenericController<Contact>
    {
        public ContactController(IGenericUnitOfWork<Contact> unit) : base(unit)
        {
        }
    }
}
