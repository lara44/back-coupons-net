using back_coupons.Data;
using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Helpers;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using Microsoft.EntityFrameworkCore;

namespace back_coupons.Repositories.Implementations
{
    public class StateRepository : GenericRepository<Entities.State>, IStateRepository
    {
        private readonly DataContext _dbContext;
        public StateRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }


        public override async Task<ActionResponse<Entities.State>> GetAsync(int id)
        {
            var row = await _dbContext.States
                .Include(s => s.Cities!)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (row != null)
            {
                return new ActionResponse<Entities.State>
                {
                    Successfully = true,
                    Result = row
                };
            }
            return new ActionResponse<Entities.State>
            {
                Successfully = false,
                Message = "Registro no encontrado"
            };
        }

        public override async Task<ActionResponse<IEnumerable<Entities.State>>> GetAsyncFull()
        {
            return new ActionResponse<IEnumerable<Entities.State>>
            {
                Successfully = true,
                Result = await _dbContext.States
                   .Include(s => s.Cities!)
                   .ToListAsync()
            };
        }

        public override async Task<ActionResponse<IEnumerable<Entities.State>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _dbContext.States
                .Where(s => !pagination.Id.HasValue || s.CountryId == pagination.Id)
                .Include(s => s.Cities!)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            var count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling((double)count / pagination.RecordsNumber);

            return new ActionResponse<IEnumerable<Entities.State>>
            {
                Successfully = true,
                Result = await queryable
                    .OrderBy(x => x.Name)
                    .Paginate(pagination)
                    .ToListAsync(),
                TotalPage = totalPages
            };
        }

        public async Task<ActionResponse<IEnumerable<State>>> GetStatesByCountryListAsync(int country)
        {
            var result = await _dbContext.States
                    .Where(s => s.CountryId == country)
                    .OrderBy(s => s.Name)
                    .ToListAsync();

            return new ActionResponse<IEnumerable<State>>
            {
                Successfully = true,
                Result = result
            };
        }
    }
}