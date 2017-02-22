using System.Collections.Generic;

namespace WebApp.Models
{
    public interface IEFRepository
    {
        IEnumerable<Contact> GetAllContacts();
    }
}