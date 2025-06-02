using MySuperStore.Models.Orders;

namespace MySuperStore.Models.Customers
{
    public class CustomerDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<OrderSummaryViewModel> Orders { get; set; } = new();
    }
}