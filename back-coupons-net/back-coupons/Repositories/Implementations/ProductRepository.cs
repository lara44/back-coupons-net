using back_coupons.Data;
using back_coupons.Entities;
using back_coupons.Exceptions;
using back_coupons.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back_coupons.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _dbContext;

        public ProductRepository(DataContext context)
        {
            _dbContext = context;
        }

        public async Task<ICollection<Product>> GetAllAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var response = await _dbContext.Products.FindAsync(id);
            if (response == null)
            {
                throw new NotFoundException($"Product con ID {id} no encontrado");
            }

            return response;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            try
            {
                _dbContext.Products.Add(product);
                await _dbContext.SaveChangesAsync();
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            try
            {
                var existingProduct = await _dbContext.Products.FindAsync(product.Id);
                if (existingProduct == null)
                {
                    throw new Exception("Usuario no encontrado");
                }

                existingProduct.Name = product.Name;
                existingProduct.Barcode = product.Barcode;
                existingProduct.Price = product.Price;
                await _dbContext.SaveChangesAsync();
                return existingProduct;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar producto", ex);
            }
        }
    }
}
