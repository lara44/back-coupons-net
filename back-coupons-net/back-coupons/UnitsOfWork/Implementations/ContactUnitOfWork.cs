using back_coupons.Entities;
using back_coupons.Repositories.Interfaces;
using back_coupons.UnitsOfWork.Interfaces;

namespace back_coupons.UnitsOfWork.Implementations
{
    public class ContactUnitOfWork : IContactUnitOfWork
    {
        private readonly IContactRepository _contactRepository;

        public ContactUnitOfWork(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<ICollection<Contact>> GetAllAsync() => await _contactRepository.GetAllAsync();
        public async Task<Contact> GetContactByIdAsync(int contactId) => await _contactRepository.GetContactByIdAsync(contactId);
        public async Task<Contact> CreateContactAsync(Contact contact) => await _contactRepository.CreateContactAsync(contact);
        public async Task<Contact> UpdateContactAsync(Contact contact) => await _contactRepository.UpdateContactAsync(contact);
    }
}
