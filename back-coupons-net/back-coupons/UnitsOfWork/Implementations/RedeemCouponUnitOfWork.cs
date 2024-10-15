using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;

namespace back_coupons.UnitsOfWork.Implementations
{
    public class RedeemCouponUnitOfWork : GenericUnitOfWork<RedeemCoupon>, IRedeemCouponUnitOfWork
    {
        private readonly IRedeemCouponRepository _redeemCouponRepository;
        public RedeemCouponUnitOfWork(IGenericRepository<RedeemCoupon> repository, IRedeemCouponRepository redeemCouponRepository) : base(repository)
        {
            _redeemCouponRepository = redeemCouponRepository;
        }
        public async Task<ActionResponse<RedeemCoupon>> RedeemCouponAsync(string code, int clientId) => await _redeemCouponRepository.RedeemCouponAsync(code, clientId);
    }
}
