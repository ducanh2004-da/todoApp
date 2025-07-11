using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using backend.Models.DTO;

namespace backend.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<TaskDTO>> GetAllAsync(string status);
        Task<TaskDTO> GetByIdAsync(int id);
        Task<int> AddAsync(TaskDTO task);
        Task UpdateAsync(TaskDTO task);
        Task DeleteAsync(int id);
        Task MarkCompleteAsync(int id);
        Task<Dictionary<string, int>> GetReportAsync();

        Task<IEnumerable<TaskCsvModel>> GetAllForCsvAsync();
    }
}
