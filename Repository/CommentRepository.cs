using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artplace.Models;
using Dapper;
using Npgsql;

namespace Artplace.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private string connectionString =
            "User ID=postgres;Password=25avg2013;Host=localhost;Port=5432;Database=art_db;";

        public async Task Add(Comment comment)
        {
            var sql = @"INSERT INTO comments (user_id, advert_id, text) VALUES (@UserId, @AdvertId, @Text)";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql, new { UserId = comment.UserId, AdvertId = comment.AdvertId, Text = comment.Text });
            }        
        }

        public async Task<List<Comment>> GetBy(int advertId)
        {
            var sql = @"SELECT c.id, user_id AS UserId, advert_id AS ReceiptId, text, username, photo AS UserImagePath 
                        FROM comments c JOIN users u on c.user_id = u.id WHERE advert_id = @AdvertId";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var result = connection.Query<Comment>(sql, new { AdvertId = advertId }).ToList();
                return result;
            }
        }
    }
}