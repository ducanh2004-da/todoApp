using System.Globalization;
using System.IO;
using System;
using System.Threading.Tasks;
using backend.Interfaces;
using backend.Models;
using backend.Models.DTO;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _repo;
        public TaskController(ITaskRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string status = "all")
        {
            var all = await _repo.GetAllAsync(status);
            return Ok(all);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var t = await _repo.GetByIdAsync(id);
            if (t == null) return NotFound();
            return Ok(t);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskDTO task)
        {
            if (string.IsNullOrWhiteSpace(task.TaskTitle))
                return BadRequest("Task title is required.");

           
            var newId = await _repo.AddAsync(task);

            
            task.TaskId = newId;
            return CreatedAtAction(nameof(Get), new { id = task.TaskId }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TaskDTO task)
        {
            var exist = await _repo.GetByIdAsync(id);
            if (exist == null) return NotFound();

            
            exist.TaskTitle = task.TaskTitle;
            exist.Description = task.Description;
            exist.Priority = task.Priority;
            exist.StartDay = task.StartDay;
            exist.EndDay = task.EndDay;

            await _repo.UpdateAsync(exist);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("complete/{id}")]
        public async Task<IActionResult> MarkComplete(int id)
        {
            await _repo.MarkCompleteAsync(id);
            return NoContent();
        }

        [HttpGet("report")]
        public async Task<IActionResult> Report()
        {
            var rpt = await _repo.GetReportAsync();
            return Ok(rpt);
        }

        [HttpGet("csvexport")]
        public async Task<IActionResult> ExportCsv()
        {
            var records = await _repo.GetAllForCsvAsync();

            // 2) Tạo MemoryStream không dùng using để giữ nó mở
            var mem = new MemoryStream();

            // 3) Ghi BOM
            using (var writer = new StreamWriter(mem, leaveOpen: true))
            {
                writer.Write('\uFEFF');
                writer.Flush();
            }

            // 4) Ghi CSV vào stream (ghi luôn lên mem, vì leaveOpen=true)
            using (var writer = new StreamWriter(mem, leaveOpen: true))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader<TaskCsvModel>();
                await csv.NextRecordAsync();
                await csv.WriteRecordsAsync(records);
                await writer.FlushAsync();
            }

            // 5) Đặt con trỏ về đầu
            mem.Position = 0;

            // 6) Trả về FileStreamResult — ASP.NET Core sẽ tự dispose mem sau khi ghi xong
            var fileName = $"tasks_{DateTime.UtcNow:yyyyMMddHHmm}.csv";
            return File(mem, "text/csv; charset=utf-8", fileName);
        }
    }
}
