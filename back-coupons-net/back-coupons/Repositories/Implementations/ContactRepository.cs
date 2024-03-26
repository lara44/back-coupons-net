using back_coupons.Data;
using back_coupons.Entities;
using back_coupons.Exceptions;
using back_coupons.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back_coupons.Repositories.Implementations
{
    public class ContactRepository : IContactRepository
    {
        private readonly DataContext _dbContext;

        public ContactRepository(DataContext context)
        {
            _dbContext = context;
        }

        public async Task<ICollection<Contact>> GetAllAsync()
        {
            return await _dbContext.Contacts.ToListAsync();
        }

        public async Task<Contact> GetContactByIdAsync(int contactId)
        {
            var response = await _dbContext.Contacts.FindAsync(contactId);
            if (response == null)
            {
                throw new NotFoundException($"Registro contacto con ID {contactId} no encontrado");
            }

            return response;
        }

        public async Task<Contact> CreateContactAsync(Contact contact)
        {
            try
            {
                _dbContext.Contacts.Add(contact);
                await _dbContext.SaveChangesAsync();
                return contact;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Contact> UpdateContactAsync(Contact contact)
        {
            try
            {
                var existingContact = await _dbContext.Contacts.FindAsync(contact.Id);
                if (existingContact == null)
                {
                    throw new Exception("Registro de contacto no encontrado");
                }

                // Actualiza las propiedades del contacto existente con las del contacto actualizado.
                existingContact.Name = contact.Name;
                existingContact.Phone = contact.Phone;
                existingContact.Address = contact.Address;
                existingContact.Email = contact.Email;
                // Guarda los cambios asincrónicamente en la base de datos.
                await _dbContext.SaveChangesAsync();
                return existingContact;
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción que pueda ocurrir durante la actualización del contacto
                // y la relanza envolviéndola en una nueva excepción con el mismo mensaje.
                throw new Exception("Error al actualizar registro de contacto", ex);
            }
        }
    }
}
