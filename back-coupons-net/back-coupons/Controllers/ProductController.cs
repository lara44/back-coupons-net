
using back_coupons.Entities;
using back_coupons.Exceptions;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_coupons.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Route("api/products")]
    public class ProductController : GenericController<Product>
    {
        public ProductController(IGenericUnitOfWork<Product> unit) : base(unit)
        {
        }
    }
}
