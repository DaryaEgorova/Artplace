using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using Artplace.Models;
using Npgsql;

namespace Artplace.Repository
{
    public class UserRepository : IUserRepository
    {
        private string connectionString = "User ID=postgres;Password=25avg2013;Host=localhost;Port=5432;Database=art_db;";
        
        public async Task<User> GetUserByEmailAndPasswordAsync(string email, string password)
        {
            await using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                //use the connection here
                var user = connection.QueryFirstOrDefault<User>(
                    @"SELECT id, username, fullname, email, password, photo AS ImagePath, role_id AS role, enter_key AS EnterKey 
                    FROM users 
                    WHERE email = @Email AND password = crypt(@Password, password)",
                    new {Email = email, Password = password}) ?? new User();
                return user;
            }
        }

        public async Task<User> GetUserByEnterKey(string enterKey)
        {
            await using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                //use the connection here
                var user = connection.QueryFirstOrDefault<User>(
                    @"SELECT id, username, fullname, email, password, photo AS ImagePath, role_id AS role, enter_key AS EnterKey 
                    FROM users 
                    WHERE enter_key = @EnterKey",
                    new { EnterKey = enterKey }) ?? new User();
                return user;
            }
        }

        public async Task AddUser(User user, string password)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                connection.Execute(
                    @"INSERT INTO users (username, fullname, email, password, photo, role_id) 
                    VALUES(@Username, @Fullname, @Email, crypt(@Password, gen_salt('bf')), @Photo, @Role)",
                    new { Username = user.Username, Fullname = user.Fullname, 
                        Email = user.Email, Password = password, Photo = user.ImagePath, Role = user.Role});
            }
        }

        public async Task<bool> IsNicknameExist(string username)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var result = connection.ExecuteScalar<bool>("SELECT COUNT(1) FROM users WHERE username = @Username",
                    new {Username = username});
                return result;
            }
        }

        public async Task<bool> IsEmailExist(string email)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var result = connection.ExecuteScalar<bool>("SELECT COUNT(1) FROM users WHERE email = @Email",
                    new {Email = email});
                return result;
            }
        }

        public async Task<User> GetById(int id)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var user = connection.QueryFirstOrDefault<User>(@"SELECT id, username, fullname, email, password, photo AS ImagePath, role_id AS role 
                FROM users WHERE id = @Id",
                    new {Id = id});
                return user;
            }
        }

        public async Task UpdateUserNickname(int userId, string newNickname)
        {
            var sql = "UPDATE users SET username = @Username WHERE id = @Id";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql, new { Username = newNickname, Id = userId});
            }
        }

        public async Task UpdateUserFullname(int userId, string newFullname)
        {
            var sql = "UPDATE users SET fullname = @Fullname WHERE id = @Id";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql, new { Fullname = newFullname, Id = userId});
            }
        }

        public async Task UpdateUserPassword(int userId, string newPassword)
        {
            var sql = "UPDATE users SET password = crypt(@Password, gen_salt('bf')) WHERE id = @Id";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql, new { Password = newPassword, Id = userId});
            }
        }

        public async Task<bool> IsPasswordMatch(int userId, string password)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var result = connection.ExecuteScalar<bool>("SELECT COUNT(1) FROM users WHERE id = @Id AND password = crypt(@Password, password)",
                    new { Id = userId, Password = password});
                return result;
            }
        }
        
        public async Task<List<User>> GetAll()
        {
            var sql =
                @"SELECT id, username, fullname, email, password, photo AS ImagePath, role_id AS role, enter_key AS EnterKey 
                    FROM users";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var result = connection.Query<User>(sql).ToList();
                return result;
            }        
        }
    }
}