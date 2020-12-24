using System.Collections.Generic;
using System.Threading.Tasks;
using Artplace.Models;

namespace Artplace.Repository
{
    public interface IAdvertRepository
    {
        Task<Advert> GetById(int id);
        Task<List<Advert>> GetAll();
        Task<List<Advert>> GetAllMatchesString(string input);
        Task Add(Advert receipt);
        Task<List<Advert>> GetAllByUserId(int id);

    }
}