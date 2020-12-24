using System.Collections.Generic;
using System.Threading.Tasks;
using Artplace.Models;

namespace Artplace.Repository
{
    public interface IQuestionRepository
    {
        Task Add(Question q);
        Task<List<Question>> GettAll();
    }
}