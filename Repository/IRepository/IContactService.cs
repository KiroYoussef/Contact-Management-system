using EntityModels.DTO;
using EntityModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IContactService
    {
       dynamic GetContacts(string name = null, string phone = null, string address = null, int page = 1, int pageSize = 5);
        int AddContact(ContactDTO contact, Guid User);
        int EditContact(ContactDTO contact);
        int DeleteContact(int ContactID);
        Contact GetContact(int ContactID);
    }
}
