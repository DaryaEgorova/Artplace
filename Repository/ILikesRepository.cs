using System.Collections.Generic;
using System.Threading.Tasks;
using Artplace.Models;

namespace Artplace.Repository
{
    public interface ILikesRepository
    {
        Task Like(int userId, int advertId);
        Task DeleteLike(int userId, int advertId);
        Task<List<Advert>> GetAllLikedBy(int userId);
        Task<bool> IsLikeExist(int userId, int advertId);
    }
}