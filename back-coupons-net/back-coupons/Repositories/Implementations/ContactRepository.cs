using back_coupons.Data;
using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Helpers;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using Microsoft.EntityFrameworkCore;

namespace back_coupons.Repositories.Implementations
{
    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        private readonly DataContext _dbContext;
        public ContactRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<ActionResponse<Contact>> GetAsync(int id)
        {
            var row = await _dbContext.Contacts
                .FirstOrDefaultAsync(c => c.Id == id);
            if (row != null)
            {
                return new ActionResponse<Contact>
                {
                    Successfully = true,
                    Result = row
                };
            }
            return new ActionResponse<Contact>
            {
                Successfully = false,
                Message = "Registro no encontrado"
            };
        }

        public override async Task<ActionResponse<IEnumerable<Contact>>> GetAsyncFull()
        {
            return new ActionResponse<IEnumerable<Contact>>
            {
                Successfully = true,
                Result = await _dbContext.Contacts
                .ToListAsync()
            };
        }

        public override async Task<ActionResponse<IEnumerable<Contact>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _dbContext.Contacts
                .Where(c => !pagination.Id.HasValue || c.CompanyId == pagination.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            var count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling((double)count / pagination.RecordsNumber);

            return new ActionResponse<IEnumerable<Contact>>
            {
                Successfully = true,
                Result = await queryable
                    .OrderBy(x => x.Name)
                    .Paginate(pagination)
                    .ToListAsync(),
                TotalPage = totalPages
            };
        }
    }
}