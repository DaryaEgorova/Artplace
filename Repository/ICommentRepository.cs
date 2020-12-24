using System.Collections.Generic;
using System.Threading.Tasks;
using Artplace.Models;

namespace Artplace.Repository
{
    public interface ICommentRepository
    {
        Task Add(Comment comment);
        Task<List<Comment>> GetBy(int advertId);
    }
}