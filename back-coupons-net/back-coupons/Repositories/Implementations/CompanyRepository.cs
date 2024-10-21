using back_coupons.Data;
using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Helpers;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using Microsoft.EntityFrameworkCore;

namespace back_coupons.Repositories.Implementations
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        private readonly DataContext _dbContext;
        public CompanyRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ActionResponse<Company>> GetCompanyByUserAsync(string userId)
        {
            var company = await _dbContext.Users
                    .Where(u => u.Id == userId)
                    .Include(u => u.Company!) 
                    .ThenInclude(c => c.Contacts)
                    .Include(u => u.Company!) 
                    .ThenInclude(c => c.Coupons) 
                    .Select(u => u.Company)
                    .FirstOrDefaultAsync();

            if (company == null)
            {
                return new ActionResponse<Company>
                {
                    Successfully = false,
                    Message = "No se encontró la empresa asociada a este usuario."
                };
            }

            return new ActionResponse<Company>
            {
                Successfully = true,
                Result =  company
            };
        }

        public override async Task<ActionResponse<Company>> GetAsync(int id)
        {
            var row = await _dbContext.Companies
                .Include(c => c.Contacts!)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (row != null)
            {
                return new ActionResponse<Company>
                {
                    Successfully = true,
                    Result = row
                };
            }
            return new ActionResponse<Company>
            {
                Successfully = false,
                Message = "Registro no encontrado"
            };
        }

        public override async Task<ActionResponse<IEnumerable<Company>>> GetAsyncFull()
        {
            return new ActionResponse<IEnumerable<Company>>
            {
                Successfully = true,
                Result = await _dbContext.Companies
                .Include(c => c.Contacts!)
                .ToListAsync()
            };
        }

        public override async Task<ActionResponse<IEnumerable<Company>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _dbContext.Companies
                .Include(c => c.Contacts!)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<Company>>
            {
                Successfully = true,
                Result = await queryable
                    .OrderBy(x => x.Name)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }
    }
}