namespace back_coupons.Entities
{
    public class DetailCoupon
    {
        public int Id { get; set; }
        public Coupon? Coupon { get; set; }
        public int CouponId { get; set; }
        public Product? Product { get; set; }
        public int ProductId { get; set; }
    }
}
