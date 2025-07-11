using backend.Interfaces;
using backend.Models;
using backend.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagTaskController : ControllerBase
    {
        private readonly ITagTaskRepository _repository;

        public TagTaskController(ITagTaskRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gán 1 tag cho task
        /// POST /api/tagtask
        /// Body: { "taskId": 1, "tagId": 2 }
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskTagDTO TagTask)
        {
            await _repository.AddTagToTaskAsync(TagTask.TaskId, TagTask.TagId);
            return Ok(new { message = "Tag đã được thêm vào task thành công." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TaskTagDTO TagTask)
        {
            await _repository.UpdateTagToTaskAsync(id, TagTask.TaskId, TagTask.TagId);
            return Ok(new { message = "Tag đã được cập nhật vào task thành công." });
        }

        /// <summary>
        /// Gỡ tag ra khỏi task
        /// DELETE /api/tagtask
        /// Body: { "taskId": 1, "tagId": 2 }
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.RemoveTagFromTaskAsync(id);
            return Ok(new { message = "Tag đã được gỡ khỏi task thành công." });
        }
    }
}
