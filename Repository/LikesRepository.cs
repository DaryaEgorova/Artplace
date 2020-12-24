using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artplace.Models;
using Dapper;
using Npgsql;

namespace Artplace.Repository
{
    public class LikesRepository : ILikesRepository
    {
        private string connectionString =
            "User ID=postgres;Password=25avg2013;Host=localhost;Port=5432;Database=art_db;";
        
        public async Task Like(int userId, int advertId)
        {
            var sql = @"INSERT INTO likes(user_id, advert_id) VALUES (@UserId, @AdvertId)";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql, new {UserId = userId, AdvertId = advertId});
            }
        }

        public async Task DeleteLike(int userId, int advertId)
        {
            var sql = @"DELETE FROM likes WHERE user_id = @UserId AND advert_id = @AdvertId";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql, new {UserId = userId, AdvertId = advertId});
            }
        }

        public async Task<List<Advert>> GetAllLikedBy(int userId)
        {
            var sql = @"SELECT id, title, text, author_id AS AuthorId
            FROM adverts JOIN likes ON likes.advert_id = adverts.id WHERE user_id = @UserId";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var result = (await connection.QueryAsync<Advert>(sql, new {UserId = userId})).ToList();
                return result;
            } 
        }

        public async Task<bool> IsLikeExist(int userId, int advertId)
        {
            var sql = @"SELECT COUNT(1) FROM likes WHERE user_id = @UserId AND advert_id = @AdvertId";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var result = connection.ExecuteScalar<bool>(sql, new {UserId = userId, AdvertId = advertId});
                return result;
            }
        }
    }
}