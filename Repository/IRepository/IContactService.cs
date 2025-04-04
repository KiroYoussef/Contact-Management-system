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
       dynamic GetContacts();
    }
}
