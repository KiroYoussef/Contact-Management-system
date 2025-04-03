using EntityModels.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IUserService
    {
        void AddDefaultUsers();

        Task<bool> Register(RegisterDTO UserInfo);
        Task<bool> Login(LoginDTO UserInfo); 
           
    }
}
