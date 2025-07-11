using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Interfaces
{
    public interface ITagRepository
    {
        Task<IEnumerable<TagEntity>> GetAllAsync();
        Task<TagEntity> GetByIdAsync(int id);
        Task AddAsync(TagEntity tag);
        Task UpdateAsync(TagEntity tag);
        Task DeleteAsync(int id);
    }
}
