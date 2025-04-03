using EntityModels.DTO;
using EntityModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly string _secretKey;

        public UserService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _secretKey = configuration["SecretKey"];
        }

        public void AddDefaultUsers()
        {
            var User1 = new User
            {
                Username = "user1",
                FirstName = "FUser1",
                MiddleName = "MUser1",
                LastName = "LUser1"
            };
            User1.HashPassword("user1", _secretKey);

            var User2 = new User
            {
                Username = "user2",
                FirstName = "FUser2",
                MiddleName = "MUser2",
                LastName = "LUser2"
            };
            User2.HashPassword("user2", _secretKey);



            if (!  _context.Users.Any(u => u.Username == User1.Username))
                 _context.Users.Add(User1);

            if (! _context.Users.Any(u => u.Username == User2.Username))
                 _context.Users.Add(User2);

             _context.SaveChanges();

        }

        public async Task<bool> Register(RegisterDTO UserInfo)
        {
            if (await _context.Users.AnyAsync(u => u.Username == UserInfo.Username))
                return false;

            var user = new User
            {
                Username = UserInfo.Username,
                LastName = UserInfo.LastName,
                MiddleName = UserInfo.MiddleName,
                FirstName = UserInfo.FirstName
            };

            user.HashPassword(UserInfo.Password, _secretKey);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserDTO> Login(LoginDTO UserInfo)
        {
            var user =await  _context.Users.FirstOrDefaultAsync(u => u.Username == UserInfo.Username);
            if (user == null) return new UserDTO(){ };

            if (user.VerifyPassword(UserInfo.Password, _secretKey))
            {
                UserDTO CurrentUser=new UserDTO();
                CurrentUser.Id = user.Id;       
                CurrentUser.Username = user.Username;
                CurrentUser.FirstName = user.FirstName;
                CurrentUser.MiddleName = user.MiddleName;
                CurrentUser.LastName = user.LastName;
                return CurrentUser;

            }
            else
                return new UserDTO() { };
        }
    }
}
