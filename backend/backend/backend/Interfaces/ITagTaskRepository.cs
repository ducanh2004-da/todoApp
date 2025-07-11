// Interfaces/ITagTaskRepository.cs
using backend.Models;
using backend.Models.DTO;
using System.Threading.Tasks;

namespace backend.Interfaces
{
    public interface ITagTaskRepository
    {
        Task AddTagToTaskAsync(int TaskId, int TagId);
        Task UpdateTagToTaskAsync(int id, int TaskId, int TagId);
        Task RemoveTagFromTaskAsync(int TaskTagId);

        //TaskTag<TaskTagDTO> GetByIdAsync(int taskId, int tagId);
    }
}
