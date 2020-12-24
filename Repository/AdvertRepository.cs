using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artplace.Models;
using Dapper;
using Npgsql;

namespace Artplace.Repository
{
    public class AdvertRepository : IAdvertRepository
    {
        private string connectionString =
            "User ID=postgres;Password=25avg2013;Host=localhost;Port=5432;Database=art_db;";
        
        public async Task<Advert> GetById(int id)
        {
            var sql =
                @"SELECT id, title, text, author_id AS AuthorId, photo AS ImagePath FROM adverts WHERE id = @Id";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var result = connection.QueryFirstOrDefault<Advert>(sql, new { Id = id });
                return result;
            } 
        }

        public async Task<List<Advert>> GetAll()
        {
            var sql =
                @"SELECT id, title, text, author_id AS AuthorId, photo AS ImagePath FROM adverts";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var result = connection.Query<Advert>(sql).ToList();
                return result;
            } 
        }

        public async Task<List<Advert>> GetAllMatchesString(string input)
        {
            var sql =
                @"SELECT id, title, text, author_id AS AuthorId, photo AS ImagePath FROM adverts WHERE lower(title) LIKE lower(@Input)";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var result = connection.Query<Advert>(sql, new {Input = "%" + input + "%"}).ToList();
                return result;
            }        
        }
        
        public async Task Add(Advert advert)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                connection.Execute(
                    @"INSERT INTO adverts(title, text, author_id, photo)
                    VALUES(@Title, @Text, @AuthorId, @Photo)",
                    new { Title = advert.Title, Text = advert.Text, 
                        AuthorId = advert.AuthorId, Photo = advert.ImagePath});
            }
        }

        public async Task<List<Advert>> GetAllByUserId(int id)
        {
            var sql =
                @"SELECT id, title, text, photo AS ImagePath FROM adverts WHERE author_id = @Id";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var result = connection.Query<Advert>(sql, new { Id = id }).ToList();
                return result;
            }
        }
    }
}