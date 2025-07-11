using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Interfaces;
using backend.Models;
using backend.Models.DTO;
using backend.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        //Khi repository được khởi tạo , ta có thể inject ctx cho phép truy cập database
        private readonly ApplicationDbContext _ctx;
        public TaskRepository(ApplicationDbContext ctx) => _ctx = ctx;

        public async Task<List<TaskDTO>> GetAllAsync(string status)
        {
            //var query = _ctx.Tasks.AsNoTracking()
            //    .Include(t => t.TaskTags)
            //        .ThenInclude(tt => tt.Tag)
            //    .AsQueryable();

            var query = _ctx.TaskTags.AsNoTracking()
                .Include(tt => tt.Task)
                .Include(tt => tt.Tag)
                .Where(x => x.TaskId == x.Task.TaskId && x.TagId == x.Tag.TagId)
                .AsQueryable();

            if (status.ToLower() == TaskEnum.DONE.ToString().ToLower())
            {
                query = query.Where(tt => tt.Task.IsDone);
            }
            else if (status.ToLower() == TaskEnum.PENDING.ToString().ToLower())
            {
                query = query.Where(tt => !tt.Task.IsDone);
            }

            var BoundList = await query.Select(tt => new TaskDTO
            {
                TaskId = tt.Task.TaskId,
                TaskTitle = tt.Task.TaskTitle,
                StartDay = tt.Task.StartDay,
                EndDay = tt.Task.EndDay,
                Description = tt.Task.Description,
                Priority = tt.Task.Priority,
                IsDone = tt.Task.IsDone,
                Color = tt.Tag.Color,
                TagName = tt.Tag.TagName,
            }).ToListAsync();

            BoundList.ToList();

            return BoundList;




            //return status.ToLower() switch
            //{
            //    "pending" => await query.Where(t => !t.IsDone).ToListAsync(),
            //    "done" => await query.Where(t => t.IsDone).ToListAsync(),
            //    _ => await query.ToListAsync(),
            //};
        }

        public async Task<TaskDTO> GetByIdAsync(int id)
        { 
            var entity = await _ctx.Tasks
                .AsNoTracking()
                .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Tag)
                .FirstOrDefaultAsync(t => t.TaskId == id);
         if (entity == null) return null;

            // Chuyển entity → DTO
            return new TaskDTO
            {
                TaskId      = entity.TaskId,
                TaskTitle   = entity.TaskTitle,
                StartDay    = entity.StartDay,
                EndDay      = entity.EndDay,
                Description = entity.Description,
                Priority    = entity.Priority,
                IsDone      = entity.IsDone,
                TagName     = entity.TaskTags.FirstOrDefault()?.Tag.TagName,
                Color       = entity.TaskTags.FirstOrDefault()?.Tag.Color
            };
        }

    public async Task<int> AddAsync(TaskDTO task)
        {

            if (string.IsNullOrWhiteSpace(task.TaskTitle))
                throw new ArgumentException("Title is required");

            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;
            var entity = new TaskEntity
            {
                TaskTitle = task.TaskTitle,
                StartDay = task.StartDay,
                EndDay = task.EndDay,
                Description = task.Description,
                Priority = task.Priority,
                IsDone = task.IsDone,
            };

            await _ctx.Tasks.AddAsync(entity);
            await _ctx.SaveChangesAsync();

            return entity.TaskId;
        }

        public async Task UpdateAsync(TaskDTO task)
        {
            var e = await _ctx.Tasks
                .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Tag)
                .FirstOrDefaultAsync(t => t.TaskId == task.TaskId);
            if (e == null) throw new KeyNotFoundException("Task not found");

            e.TaskTitle = task.TaskTitle;
            e.StartDay = task.StartDay;
            e.EndDay = task.EndDay;
            e.Description = task.Description;
            e.Priority = task.Priority;
            e.UpdatedAt = DateTime.UtcNow;

            _ctx.Tasks.Update(e);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _ctx.Tasks.FindAsync(id);
            if (e != null)
            {
                _ctx.Tasks.Remove(e);
                await _ctx.SaveChangesAsync();
            }
        }

        public async Task MarkCompleteAsync(int id)
        {
            var e = await _ctx.Tasks.FindAsync(id);
            if (e != null)
            {
                e.IsDone = true;
                e.UpdatedAt = DateTime.UtcNow;
                await _ctx.SaveChangesAsync();
            }
        }

        public async Task<Dictionary<string, int>> GetReportAsync()
        {
            var total = await _ctx.Tasks.CountAsync();
            var done = await _ctx.Tasks.CountAsync(t => t.IsDone);
            var pending = total - done;
            var high = await _ctx.Tasks.CountAsync(t => t.Priority == "High");
            var med = await _ctx.Tasks.CountAsync(t => t.Priority == "Medium");
            var low = await _ctx.Tasks.CountAsync(t => t.Priority == "Low");

            return new Dictionary<string, int>
            {
                ["TotalTasks"] = total,
                ["DoneTasks"] = done,
                ["PendingTasks"] = pending,
                ["HighPriority"] = high,
                ["MediumPriority"] = med,
                ["LowPriority"] = low
            };
        }
        public async Task<IEnumerable<TaskCsvModel>> GetAllForCsvAsync()
        {
            return await _ctx.Tasks
                .Select(t => new TaskCsvModel
                {
                    TaskId = t.TaskId,
                    TaskTitle = t.TaskTitle,
                    Description = t.Description,
                    Priority = t.Priority,
                    StartDay = t.StartDay,
                    EndDay = t.EndDay,
                    IsDone = t.IsDone
                })
                .ToListAsync();
        }

    }
}
