using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagRepository _repository;

        public TagController(ITagRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tags = await _repository.GetAllAsync();
            return Ok(tags);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TagEntity tag)
        {
            await _repository.AddAsync(tag);
            return Ok("added successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TagEntity tag)
        {
            tag.TagId = id;
            await _repository.UpdateAsync(tag);
            return Ok("updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return Ok("deleted successfully");
        }
    }
}
