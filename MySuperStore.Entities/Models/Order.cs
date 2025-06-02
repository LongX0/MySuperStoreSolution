using MySuperStore.Entities.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.Now;
    public ICollection<OrderProduct> OrderProducts { get; set; }

    [NotMapped]
    public decimal TotalPrice => OrderProducts?.Sum(op => op.Product.Price * op.Quantity) ?? 0;
}
