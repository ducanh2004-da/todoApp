using System.Threading.Tasks;
using backend.Data;
using backend.Interfaces;
using backend.Models;
using backend.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class TagTaskRepository : ITagTaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TagTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddTagToTaskAsync(int TaskId, int TagId)
        {
            // Tạo entity liên kết
            var taskTag = new TaskTag
            {
                TaskId = TaskId,
                TagId = TagId
            };

            _context.TaskTags.Add(taskTag);
            await _context.SaveChangesAsync();
        }

        //public async TaskTag<TaskTagDTO> GetByIdAsync(int taskId, int tagId)
        //{ 
        //    var entity = await _context.TaskTags
        //        .FindAsync(taskId, tagId);

        //    if (entity == null)
        //    {
        //        return null;
        //    }
        //    return new TaskTagDTO { TaskId = entity.TaskId, TagId = entity.TagId };
        //}

        public async Task UpdateTagToTaskAsync(int TaskTagId, int TaskId, int TagId)
        {
            // Tìm bản ghi theo khóa chính composite
            var taskTag = await _context.TaskTags
                .FindAsync(TaskTagId);
            if (taskTag == null) return;
            // Tạo entity liên kết
                taskTag.TaskId = TaskId;
                taskTag.TagId = TagId;

            _context.TaskTags.Update(taskTag);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTagFromTaskAsync(int TaskTagId)
        {
            // Tìm bản ghi theo khóa chính composite
            var taskTag = await _context.TaskTags
                .FindAsync(TaskTagId);

            if (taskTag != null)
            {
                _context.TaskTags.Remove(taskTag);
                await _context.SaveChangesAsync();
            }
            // Nếu không tìm thấy, có thể bỏ qua hoặc ném exception tùy nhu cầu
        }
    }
}
