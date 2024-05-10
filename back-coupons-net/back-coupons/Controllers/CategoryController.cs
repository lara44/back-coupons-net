using back_coupons.Entities;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_coupons.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/categories")]
    public class CategoryController : GenericController<Category>
    {
        public CategoryController(IGenericUnitOfWork<Category> unit) : base(unit)
        {
        }
    }
}
