using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;

namespace back_coupons.UnitsOfWork.Implementations
{
    public class CouponUnitOfWork : GenericUnitOfWork<Coupon>, ICouponUnitOfWork
    {
        private readonly ICouponRepository _couponRepository;
        public CouponUnitOfWork(IGenericRepository<Coupon> repository, ICouponRepository couponRepository) : base(repository)
        {
            _couponRepository = couponRepository;
        }

        public override async Task<ActionResponse<IEnumerable<Coupon>>> GetAsyncFull() => await _couponRepository.GetAsyncFull();
        public override async Task<ActionResponse<IEnumerable<Coupon>>> GetAsync(PaginationDTO pagination) => await _couponRepository.GetAsync(pagination);
        public override async Task<ActionResponse<Coupon>> GetAsync(int id) => await _couponRepository.GetAsync(id);
        public async Task<ActionResponse<Coupon>> RedeemCouponAsync(string code) => await _couponRepository.RedeemCouponAsync(code);
    }
}
