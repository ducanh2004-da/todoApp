using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _ctx;
        public TagRepository(ApplicationDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<TagEntity>> GetAllAsync() =>
            await _ctx.Tags.ToListAsync();

        public async Task<TagEntity> GetByIdAsync(int id) =>
            await _ctx.Tags.FindAsync(id);

        public async Task AddAsync(TagEntity tag)
        {
            await _ctx.Tags.AddAsync(tag);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(TagEntity tag)
        {
            _ctx.Tags.Update(tag);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _ctx.Tags.FindAsync(id);
            if (e != null)
            {
                _ctx.Tags.Remove(e);
                await _ctx.SaveChangesAsync();
            }
        }
    }
}
