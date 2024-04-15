using back_coupons.Entities;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_coupons.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : GenericController<City>
    {
        public CityController(IGenericUnitOfWork<City> unit) : base(unit)
        {
        }
    }
}
