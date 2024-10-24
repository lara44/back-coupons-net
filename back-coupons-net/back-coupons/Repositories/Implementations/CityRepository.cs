using back_coupons.Data;
using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Helpers;
using back_coupons.Migrations;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using Microsoft.EntityFrameworkCore;

namespace back_coupons.Repositories.Implementations
{
    public class CityRepository : GenericRepository<Entities.City>, ICityRepository
    {
        private readonly DataContext _dbContext;
        public CityRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<ActionResponse<Entities.City>> GetAsync(int id)
        {
            var row = await _dbContext.Cities
                .FirstOrDefaultAsync(c => c.Id == id);
            if (row != null)
            {
                return new ActionResponse<Entities.City>
                {
                    Successfully = true,
                    Result = row
                };
            }
            return new ActionResponse<Entities.City>
            {
                Successfully = false,
                Message = "Registro no encontrado"
            };
        }

        public override async Task<ActionResponse<IEnumerable<Entities.City>>> GetAsyncFull()
        {
            return new ActionResponse<IEnumerable<Entities.City>>
            {
                Successfully = true,
                Result = await _dbContext.Cities
                .ToListAsync()
            };
        }

        public override async Task<ActionResponse<IEnumerable<Entities.City>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _dbContext.Cities
                .Where(c => !pagination.Id.HasValue || c.StateId == pagination.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            var count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling((double)count / pagination.RecordsNumber);

            return new ActionResponse<IEnumerable<Entities.City>>
            {
                Successfully = true,
                Result = await queryable
                    .OrderBy(x => x.Name)
                    .Paginate(pagination)
                    .ToListAsync(),
                TotalPage = totalPages
            };
        }

        public async Task<ActionResponse<IEnumerable<Entities.City>>> GetCitiesByStatesAsync(int state)
        {
            var result = await _dbContext.Cities
                   .Where(c => c.StateId == state)
                   .OrderBy(s => s.Name)
                   .ToListAsync();

            return new ActionResponse<IEnumerable<Entities.City>>
            {
                Successfully = true,
                Result = result
            };
        }
    }
}