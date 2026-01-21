using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.DTO;

namespace BusinessLayer.Interfaces.Services
{
    public interface IItemService
    {
        Task<List<ItemDTO>> GetAllAsync();
        Task<ItemDTO> GetByIdAsync(Guid itemId);
    }
}
