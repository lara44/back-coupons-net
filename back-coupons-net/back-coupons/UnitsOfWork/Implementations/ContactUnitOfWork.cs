using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using back_coupons.UnitsOfWork.Interfaces;

namespace back_coupons.UnitsOfWork.Implementations
{
    public class ContactUnitOfWork : GenericUnitOfWork<Contact>, IContactUnitOfWork
    {
        private readonly IContactRepository _contactRepository;
        public ContactUnitOfWork(IGenericRepository<Contact> repository, IContactRepository contactRepository) : base(repository)
        {
            _contactRepository = contactRepository;
        }

        public override async Task<ActionResponse<IEnumerable<Contact>>> GetAsyncFull() => await _contactRepository.GetAsyncFull();
        public override async Task<ActionResponse<Contact>> GetAsync(int id) => await _contactRepository.GetAsync(id);
        public override async Task<ActionResponse<IEnumerable<Contact>>> GetAsync(PaginationDTO pagination) => await _contactRepository.GetAsync(pagination);
    }
}
