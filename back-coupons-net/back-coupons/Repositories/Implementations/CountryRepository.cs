﻿using back_coupons.Data;
using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Helpers;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using Microsoft.EntityFrameworkCore;

namespace back_coupons.Repositories.Implementations
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly DataContext _dbContext;
        public CountryRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<ActionResponse<Country>> GetAsync(int id)
        {
            var row = await _dbContext.Countries
                .Include(c => c.States!)
                .ThenInclude(s => s.Cities!)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (row != null)
            {
                return new ActionResponse<Country>
                {
                    Successfully = true,
                    Result = row
                };
            }
            return new ActionResponse<Country>
            {
                Successfully = false,
                Message = "Registro no encontrado"
            };
        }

        public override async Task<ActionResponse<IEnumerable<Country>>> GetAsyncFull()
        {
            return new ActionResponse<IEnumerable<Country>>
            {
                Successfully = true,
                Result = await _dbContext.Countries
                .Include(c => c.States!)
                .ThenInclude(s => s.Cities)
                .ToListAsync()
            };
        }

        public override async Task<ActionResponse<IEnumerable<Country>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _dbContext.Countries
                .Include(c => c.States!)
                .ThenInclude(s => s.Cities)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            var count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling((double)count / pagination.RecordsNumber);

            return new ActionResponse<IEnumerable<Country>>
            {
                Successfully = true,
                Result = await queryable
                    .OrderBy(x => x.Name)
                    .Paginate(pagination)
                    .ToListAsync(),
                TotalPage = totalPages
            };
        }

        public async Task<ActionResponse<IEnumerable<Country>>> GetCountryListAsync()
        {
            var result = await _dbContext.Countries.OrderBy(c => c.Name).ToListAsync();
            return new ActionResponse<IEnumerable<Country>>
            {
                
                Successfully = true,
                Result = result
            };
        }
    }
}