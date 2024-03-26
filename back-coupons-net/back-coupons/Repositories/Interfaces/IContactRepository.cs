using back_coupons.Entities;

namespace back_coupons.Repositories.Interfaces
{
    public interface IContactRepository
    {
        Task<ICollection<Contact>> GetAllAsync();
        Task<Contact> GetContactByIdAsync(int contactId);
        Task<Contact> CreateContactAsync(Contact contact);
        Task<Contact> UpdateContactAsync(Contact contact);
    }
}
