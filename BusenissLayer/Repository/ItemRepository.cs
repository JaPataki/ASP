using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserApp.DataLayer;
using UserApp.DataLayer.Entities;
using BusinessLayer.Interfaces.Repository;

namespace BusinessLayer.Repository
{
    public class ItemRepository : BaseRepository<ItemEntity>, IItemRepository
    {
        private readonly AppDbContext _context;

        public ItemRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ItemEntity>> GetAllAsync()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<ItemEntity?> GetByItemIdAsync(Guid itemId)
        {
            return await _context.Items.FirstOrDefaultAsync(i => i.ItemId == itemId);
        }
    }
}
