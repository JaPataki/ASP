using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interfaces.Repository;
using BusinessLayer.Interfaces.Services;
using Common.DTO;
using UserApp.DataLayer.Entities;

namespace BusinessLayer.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<List<ItemDTO>> GetAllAsync()
        {
            var items = await _itemRepository.GetAllAsync();
            return items.Select(i => new ItemDTO
            {
                ItemId = i.ItemId,
                Name = i.Name,
                Description = i.Description,
                Price = i.Price,
                StockQuantity = i.StockQuantity
            }).ToList();
        }

        public async Task<ItemDTO> GetByIdAsync(Guid itemId)
        {
            var item = await _itemRepository.GetByItemIdAsync(itemId);
            return item == null ? null : new ItemDTO
            {
                ItemId = item.ItemId,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                StockQuantity = item.StockQuantity
            };
        }
    }
}
