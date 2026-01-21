using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BusinessLayer.Interfaces.Repository;
using BusinessLayer.Interfaces.Services;
using Common.DTO;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private const string SessionKey = "ShoppingCart";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IItemRepository _itemRepository;

        public ShoppingCartService(IHttpContextAccessor httpContextAccessor, IItemRepository itemRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _itemRepository = itemRepository;
        }

        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public async Task AddToCartAsync(Guid itemId, int quantity)
        {
            var cartJson = Session.GetString(SessionKey);
            List<CartItemDTO> cart;
            if (string.IsNullOrEmpty(cartJson))
                cart = new List<CartItemDTO>();
            else
                cart = JsonSerializer.Deserialize<List<CartItemDTO>>(cartJson) ?? new List<CartItemDTO>();

            var existing = cart.FirstOrDefault(c => c.ItemId == itemId);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                var item = await _itemRepository.GetByItemIdAsync(itemId);
                if (item == null) return;
                cart.Add(new CartItemDTO
                {
                    ItemId = item.ItemId,
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = quantity
                });
            }

            Session.SetString(SessionKey, JsonSerializer.Serialize(cart));
        }

        public Task<List<CartItemDTO>> GetCartItemsAsync()
        {
            var cartJson = Session.GetString(SessionKey);
            if (string.IsNullOrEmpty(cartJson)) return Task.FromResult(new List<CartItemDTO>());
            var cart = JsonSerializer.Deserialize<List<CartItemDTO>>(cartJson) ?? new List<CartItemDTO>();
            return Task.FromResult(cart);
        }

        public Task ClearCartAsync()
        {
            Session.Remove(SessionKey);
            return Task.CompletedTask;
        }
    }
}
