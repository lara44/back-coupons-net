﻿using back_coupons.DTOs;
using back_coupons.DTOs.Response;
using back_coupons.Entities;
using back_coupons.Enums;
using back_coupons.Responses;

namespace back_coupons.Repositories.Interfaces
{
    public interface IRedeemCouponRepository
    {
        Task<ActionResponse<RedeemCoupon>> RedeemCouponAsync(int couponId, int clientId, string signature);
        Task<ActionResponse<RedeemCoupon>> ClaimCustomerCouponAsync(string codeCoupon, int clientId);
        Task<ActionResponse<IEnumerable<ClaimedCouponClientDto>>> GetCouponsByClientAsync(string identification);
    }
}
