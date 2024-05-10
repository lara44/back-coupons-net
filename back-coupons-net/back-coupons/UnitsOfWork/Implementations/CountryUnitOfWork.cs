using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using back_coupons.UnitsOfWork.Interfaces;

namespace back_coupons.UnitsOfWork.Implementations
{
    public class CountryUnitOfWork : GenericUnitOfWork<Country>, ICountryUnitOfWork
    {
        private readonly ICountryRepository _countryRepository;
        public CountryUnitOfWork(IGenericRepository<Country> repository, ICountryRepository countryRepository) : base(repository)
        {
            _countryRepository = countryRepository;
        }

        public override async Task<ActionResponse<IEnumerable<Country>>> GetAsyncFull() => await _countryRepository.GetAsyncFull();
        public override async Task<ActionResponse<Country>> GetAsync(int id) => await _countryRepository.GetAsync(id);
        public override async Task<ActionResponse<IEnumerable<Country>>> GetAsync(PaginationDTO pagination) => await _countryRepository.GetAsync(pagination);
        public async Task<ActionResponse<IEnumerable<Country>>> GetCountryListAsync() => await _countryRepository.GetCountryListAsync();
    }
}
