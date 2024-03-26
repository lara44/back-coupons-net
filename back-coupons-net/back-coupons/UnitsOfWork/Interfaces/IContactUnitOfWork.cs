using back_coupons.Entities;

namespace back_coupons.UnitsOfWork.Interfaces

{
    public interface IContactUnitOfWork
    {
        Task<ICollection<Contact>> GetAllAsync();
        Task<Contact> GetContactByIdAsync(int contactId);
        Task<Contact> CreateContactAsync(Contact contact);
        Task<Contact> UpdateContactAsync(Contact contact);
    }
}
