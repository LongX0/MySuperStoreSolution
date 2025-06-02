using System.Collections.Generic;

namespace MySuperStore.Models.Orders
{
    public class OrderCreateViewModel
    {
        public int CustomerId { get; set; }

        public List<int> ProductIds { get; set; } = new List<int>();

        // ⬇️ Правильный тип — Dictionary: ProductId -> Quantity
        public Dictionary<int, int> Quantities { get; set; } = new Dictionary<int, int>();
    }
}
