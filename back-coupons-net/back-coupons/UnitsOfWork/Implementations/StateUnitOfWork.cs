using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Repositories.Implementations;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using back_coupons.UnitsOfWork.Interfaces;

namespace back_coupons.UnitsOfWork.Implementations
{
    public class StateUnitOfWork : GenericUnitOfWork<State>, IStateUnitOfWork
    {
        private readonly IStateRepository _stateRepository;
        public StateUnitOfWork(IGenericRepository<State> repository, IStateRepository stateRepository) : base(repository)
        {
            _stateRepository = stateRepository;
        }

        public override async Task<ActionResponse<IEnumerable<State>>> GetAsyncFull() => await _stateRepository.GetAsyncFull();
        public override async Task<ActionResponse<State>> GetAsync(int id) => await _stateRepository.GetAsync(id);
        public override async Task<ActionResponse<IEnumerable<State>>> GetAsync(PaginationDTO pagination) => await _stateRepository.GetAsync(pagination);
    }
}
