using back_coupons.DTOs;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using back_coupons.UnitsOfWork.Interfaces;

namespace back_coupons.UnitsOfWork.Implementations
{
    public class StateUnitOfWork : GenericUnitOfWork<Entities.State>, IStateUnitOfWork
    {
        private readonly IStateRepository _stateRepository;
        public StateUnitOfWork(IGenericRepository<Entities.State> repository, IStateRepository stateRepository) : base(repository)
        {
            _stateRepository = stateRepository;
        }

        public override async Task<ActionResponse<IEnumerable<Entities.State>>> GetAsyncFull() => await _stateRepository.GetAsyncFull();
        public override async Task<ActionResponse<Entities.State>> GetAsync(int id) => await _stateRepository.GetAsync(id);
        public override async Task<ActionResponse<IEnumerable<Entities.State>>> GetAsync(PaginationDTO pagination) => await _stateRepository.GetAsync(pagination);
        public async Task<ActionResponse<IEnumerable<Entities.State>>> GetStatesByCountryListAsync(int country) => await _stateRepository.GetStatesByCountryListAsync(country);
    }
}
