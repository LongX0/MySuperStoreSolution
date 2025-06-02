namespace MySuperStore.Models.Orders;

public class OrderSummaryViewModel
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public int ProductCount { get; set; }
    public decimal Total { get; set; }
}
