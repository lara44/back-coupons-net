
using back_coupons.Entities;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_coupons.Controllers
{

    [ApiController]
    [Route("api/products")]
    public class ProductController : GenericController<Product>
    {
        public ProductController(IGenericUnitOfWork<Product> unit) : base(unit)
        {
        }
    }
}
