using MySuperStore.Models.Products;

namespace MySuperStore.Models.Orders;

public class OrderListViewModel
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public DateTime OrderDate { get; set; }

    public List<ProductItemViewModel> Products { get; set; }
}
