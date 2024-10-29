using back_coupons.Data;
using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Helpers;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using Microsoft.EntityFrameworkCore;

namespace back_coupons.Repositories.Implementations
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        private readonly DataContext _dbContext;
        public ClientRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<ActionResponse<Client>> GetAsync(int id)
        {
            var row = await _dbContext.Clients
                .FirstOrDefaultAsync(c => c.Id == id);
            if (row != null)
            {
                return new ActionResponse<Client>
                {
                    Successfully = true,
                    Result = row
                };
            }
            return new ActionResponse<Client>
            {
                Successfully = false,
                Message = "Registro no encontrado"
            };
        }

        public async Task<ActionResponse<Client>> GetClientByIdentificationAsync(string identification)
        {
            var row = await _dbContext.Clients
                .FirstOrDefaultAsync(c => c.Identification == identification);
            if (row != null)
            {
                return new ActionResponse<Client>
                {
                    Successfully = true,
                    Result = row
                };
            }
            return new ActionResponse<Client>
            {
                Successfully = false,
                Message = "Registro no encontrado"
            };
        }

        public override async Task<ActionResponse<IEnumerable<Client>>> GetAsyncFull()
        {
            return new ActionResponse<IEnumerable<Client>>
            {
                Successfully = true,
                Result = await _dbContext.Clients
                .ToListAsync()
            };
        }

        public override async Task<ActionResponse<IEnumerable<Client>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _dbContext.Clients
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.LastName.ToLower().Contains(pagination.Filter.ToLower()));
            }

            var count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling((double)count / pagination.RecordsNumber);

            return new ActionResponse<IEnumerable<Client>>
            {
                Successfully = true,
                Result = await queryable
                    .OrderBy(x => x.FirstName)
                    .Paginate(pagination)
                    .ToListAsync(),
                TotalPage = totalPages
            };
        }
    }
}