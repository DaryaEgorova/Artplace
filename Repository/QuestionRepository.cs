using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artplace.Models;
using Dapper;
using Npgsql;

namespace Artplace.Repository
{
    public class QuestionRepository : IQuestionRepository
    {
        private string connectionString =
            "User ID=postgres;Password=25avg2013;Host=localhost;Port=5432;Database=art_db;";
        
        public async Task Add(Question q)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                connection.Execute(
                    @"INSERT INTO questions(user_id, title, text)
                    VALUES(@UserId, @Title, @Text)",
                    new { UserId = q.UserId, Title = q.Title, Text = q.Text });
            }
        }

        public async Task<List<Question>> GettAll()
        {
            var sql =
                @"SELECT user_id as UserId, title, text FROM questions";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var result = connection.Query<Question>(sql).ToList();
                return result;
            }         
        }
    }
}