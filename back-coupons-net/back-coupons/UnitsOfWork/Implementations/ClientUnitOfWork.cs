using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using back_coupons.UnitsOfWork.Interfaces;

namespace back_coupons.UnitsOfWork.Implementations
{
    public class ClientUnitOfWork : GenericUnitOfWork<Client>, IClientUnitOfWork
    {
        private readonly IClientRepository _clientRepository;
        public ClientUnitOfWork(IGenericRepository<Client> repository, IClientRepository clientRepository) : base(repository)
        {
            _clientRepository = clientRepository;
        }

        public override async Task<ActionResponse<IEnumerable<Client>>> GetAsyncFull() => await _clientRepository.GetAsyncFull();
        public override async Task<ActionResponse<Client>> GetAsync(int id) => await _clientRepository.GetAsync(id);
        public  async Task<ActionResponse<Client>> GetClientByIdentificationAsync(string identification) => await _clientRepository.GetClientByIdentificationAsync(identification);
        public override async Task<ActionResponse<IEnumerable<Client>>> GetAsync(PaginationDTO pagination) => await _clientRepository.GetAsync(pagination);
    }
}
