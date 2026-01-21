using System;
using System.Collections.Generic;
using Common.DTO;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces.Services
{
    public interface IShoppingCartService
    {
        Task AddToCartAsync(Guid itemId, int quantity);
        Task<List<CartItemDTO>> GetCartItemsAsync();
        Task ClearCartAsync();
    }
}
