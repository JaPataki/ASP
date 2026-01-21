using System;

namespace Common.DTO
{
    public class CartItemDTO
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
