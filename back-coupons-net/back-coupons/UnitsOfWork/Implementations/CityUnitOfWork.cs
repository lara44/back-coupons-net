using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using back_coupons.UnitsOfWork.Interfaces;

namespace back_coupons.UnitsOfWork.Implementations
{
    public class CityUnitOfWork : GenericUnitOfWork<City>, ICityUnitOfWork
    {
        private readonly ICityRepository _cityRepository;
        public CityUnitOfWork(IGenericRepository<City> repository, ICityRepository cityRepository) : base(repository)
        {
            _cityRepository = cityRepository;
        }

        public override async Task<ActionResponse<IEnumerable<City>>> GetAsyncFull() => await _cityRepository.GetAsyncFull();
        public override async Task<ActionResponse<City>> GetAsync(int id) => await _cityRepository.GetAsync(id);
        public override async Task<ActionResponse<IEnumerable<City>>> GetAsync(PaginationDTO pagination) => await _cityRepository.GetAsync(pagination);
    }
}
