using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Artplace.Models;

namespace Artplace.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAndPasswordAsync(string email, string password);
        Task<User> GetUserByEnterKey(string enterKey);
        Task AddUser(User user, string password);
        Task<bool> IsNicknameExist(string nickname);
        Task<bool> IsEmailExist(string email);
        Task<User> GetById(int id);
        Task UpdateUserNickname(int userId, string newNickname);
        Task UpdateUserFullname(int userId, string newFullname);
        Task UpdateUserPassword(int userId, string newPassword);
        Task<bool> IsPasswordMatch(int userId, string password);
        Task<List<User>> GetAll();
    }
}