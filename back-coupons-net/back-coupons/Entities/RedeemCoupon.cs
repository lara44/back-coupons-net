using back_coupons.Enums;

namespace back_coupons.Entities
{
    public class RedeemCoupon
    {
        public int Id { get; set; }
        public DateTime DateRedeem { get; set; }
        public Coupon? Coupon { get; set; }
        public int CouponId { get; set; }
        public Client? Client { get; set; }
        public int ClientId { get; set; }
        public RedeemState State { get; set; }
    }
}
