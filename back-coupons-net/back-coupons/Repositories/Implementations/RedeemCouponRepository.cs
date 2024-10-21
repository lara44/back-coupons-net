using System.Security.Cryptography;
using System.Text;
using back_coupons.Data;
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
            var url = $"https://example.com/redeem?couponId={couponId}&clientId={clientId}";

            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(url)));
                return $"{url}&signature={hash}";  // Devuelve la URL firmada
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
            var url = $"https://example.com/redeem?couponId={couponId}&clientId={clientId}";

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

            // Devolver respuesta exitosa
            return new ActionResponse<RedeemCoupon>
            {
                Successfully = true,
                Result = redeemCoupon
            };
        }

        public async Task<ActionResponse<RedeemCoupon>> ClaimCustomerCouponAsync(string code, int clientId)
        {
            // Verificar si el cliente existe
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
                // Si no hay más cupones disponibles
                return new ActionResponse<RedeemCoupon>
                {
                    Successfully = false,
                    Message = "No more coupons available."
                };
            }
        }

        public async Task<ActionResponse<IEnumerable<RedeemCoupon>>> GetCouponsByClientAsync(string identification)
        {
            // Realizar la consulta directamente en la tabla RedeemCoupons uniendo con Clients
            var coupons = await _dbContext.RedeemCoupons
                .Where(rc => rc.Client.Identification == identification) // Filtrar por identificación del cliente
                .Include(rc => rc.Coupon) // Incluir la entidad Coupon
                .ThenInclude(c => c.DetailCoupons) // Incluir los detalles del cupón
                .ThenInclude(dc => dc.Product) // Incluir los productos asociados
                .ToListAsync();

            // Verificar si se encontraron cupones
            if (coupons == null || coupons.Count == 0)
            {
                return new ActionResponse<IEnumerable<RedeemCoupon>>
                {
                    Successfully = false,
                    Message = "No hay cupones para este cliente"
                };
            }

            // Retornar la lista de cupones
            return new ActionResponse<IEnumerable<RedeemCoupon>>
            {
                Successfully = true,
                Result = coupons
            };
        }
    }
}