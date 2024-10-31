using System.Data;
using System.Security.Cryptography;
using System.Text;
using back_coupons.Data;
using back_coupons.DTOs.Response;
using back_coupons.Entities;
using back_coupons.Enums;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using Microsoft.EntityFrameworkCore;

namespace back_coupons.Repositories.Implementations
{
    public class RedeemCouponRepository : IRedeemCouponRepository
    {
        private readonly DataContext _dbContext;
        public RedeemCouponRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string GenerateSignedUrl(int couponId, int clientId)
        {
            var secretKey = "your_secret_key";
            var url = $"http://localhost:5173/coupons/redeem?couponId={couponId}&clientId={clientId}";

            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(url)));
                var encodedHash = Uri.EscapeDataString(hash); // Codifica el hash para que sea seguro en la URL
                return $"{url}&signature={encodedHash}";  // Devuelve la URL firmada con la firma codificada
            }
        }

        public bool VerifySignedUrl(string url, string signature)
        {
            var secretKey = "your_secret_key";  // Debe coincidir con el secreto usado para generar la firma
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(url)));
                return hash == signature;  // Comparar la firma generada con la recibida
            }
        }

        public async Task<ActionResponse<RedeemCoupon>> RedeemCouponAsync(int couponId, int clientId, string signature)
        {
            // Reconstruir la URL original sin la firma para verificarla
            var url = $"http://localhost:5173/coupons/redeem?couponId={couponId}&clientId={clientId}";

            // Verificar la firma
            if (!VerifySignedUrl(url, signature))
            {
                return new ActionResponse<RedeemCoupon>
                {
                    Successfully = false,
                    Message = "Invalid or tampered URL signature."
                };
            }

            // Buscar el registro en RedeemCoupon por couponId y clientId
            var redeemCoupon = await _dbContext.RedeemCoupons
                .Where(rc => rc.CouponId == couponId && rc.ClientId == clientId)
                .FirstOrDefaultAsync();

            if (redeemCoupon == null)
            {
                return new ActionResponse<RedeemCoupon>
                {
                    Successfully = false,
                    Message = "Redemption not found for the given coupon and client."
                };
            }

            // Verificar si el cupón ya ha sido canjeado
            if (redeemCoupon.State == RedeemState.Completed)
            {
                return new ActionResponse<RedeemCoupon>
                {
                    Successfully = false,
                    Message = "Coupon has already been redeemed."
                };
            }

            // Actualizar el estado a Completado
            redeemCoupon.State = RedeemState.Completed;
            await _dbContext.SaveChangesAsync();

            return new ActionResponse<RedeemCoupon>
            {
                Successfully = true,
                Result = redeemCoupon
            };
        }

        public async Task<ActionResponse<RedeemCoupon>> ClaimCustomerCouponAsync(string code, int clientId)
        {
            var client = await _dbContext.Clients
                .Where(c => c.Id == clientId)
                .FirstOrDefaultAsync();

            if (client == null)
            {
                return new ActionResponse<RedeemCoupon>
                {
                    Successfully = false,
                    Message = "Client not found."
                };
            }

            // Buscar el cupón por código
            var coupon = await _dbContext.Coupons
                .Include(dc => dc.DetailCoupons!)
                .ThenInclude(p => p.Product)
                .Where(c => c.CouponCode == code)
                .OrderBy(s => s.Name)
                .FirstOrDefaultAsync();

            if (coupon == null)
            {
                return new ActionResponse<RedeemCoupon>
                {
                    Successfully = false,
                    Message = "Coupon not found."
                };
            }

            // Verificar si el cliente ya tiene este cupón en la tabla RedeemCoupon
            var existingRedeemCoupon = await _dbContext.RedeemCoupons
                .Where(rc => rc.ClientId == clientId && rc.CouponId == coupon.Id)
                .FirstOrDefaultAsync();

            if (existingRedeemCoupon != null)
            {
                // Si ya existe un registro de redención para este cupón y cliente, devolver error
                return new ActionResponse<RedeemCoupon>
                {
                    Successfully = false,
                    Message = "Client already has this coupon."
                };
            }

            // Verificar si hay cantidad disponible
            if (coupon.QuantityActual > 0)
            {
                coupon.QuantityActual--;

                var url = GenerateSignedUrl(coupon.Id, clientId);

                // Crear el registro de redención del cupón
                var redeemCoupon = new RedeemCoupon
                {
                    CouponId = coupon.Id,
                    ClientId = clientId,
                    DateRedeem = DateTime.UtcNow,
                    State = RedeemState.Pending,
                    UrlCoupon = url
                };

                _dbContext.RedeemCoupons.Add(redeemCoupon);
                await _dbContext.SaveChangesAsync();

                return new ActionResponse<RedeemCoupon>
                {
                    Successfully = true,
                    Result = redeemCoupon
                };
            }
            else
            {
                return new ActionResponse<RedeemCoupon>
                {
                    Successfully = false,
                    Message = "No more coupons available."
                };
            }
        }

        public async Task<ActionResponse<IEnumerable<ClaimedCouponClientDto>>> GetCouponsByClientAsync(string identification)
        {
            var coupons = await _dbContext.RedeemCoupons
              .Where(rc => rc.Client!.Identification == identification)
              .Select(rc => new ClaimedCouponClientDto
              {
                  Id = rc.Id,
                  Url = rc.UrlCoupon,
                  Coupon = new CouponClientDto
                  {
                      Id = rc.Coupon!.Id,
                      Name = rc.Coupon.Name,
                      CouponCode = rc.Coupon.CouponCode,
                      StartDate = rc.Coupon.StartDate,
                      ExpiryDate = rc.Coupon.ExpiryDate,
                      Photo = rc.Coupon.Photo,
                      DetailCoupons = rc.Coupon.DetailCoupons!.Select(dc => new DetailCouponDto
                      {
                          Id = dc.Id,
                          CouponId = dc.CouponId,
                          Product = new ProductDto
                          {
                              Id = dc.Product!.Id,
                              Name = dc.Product.Name
                          }
                      }).ToList()
                  },
                  Client = new ClientBasicDto
                  {
                      Id = rc.Client!.Id,
                      FirstName = rc.Client.FirstName,
                      LastName = rc.Client.LastName,
                      Email = rc.Client.Email,
                      Phone = rc.Client.Phone
                  }
              })
              .ToListAsync();

            if (coupons == null || coupons.Count == 0)
            {
                return new ActionResponse<IEnumerable<ClaimedCouponClientDto>>
                {
                    Successfully = false,
                    Message = "No hay cupones para este cliente"
                };
            }

            // Retornar la lista de cupones
            return new ActionResponse<IEnumerable<ClaimedCouponClientDto>>
            {
                Successfully = true,
                Result = coupons
            };
        }
    }
}