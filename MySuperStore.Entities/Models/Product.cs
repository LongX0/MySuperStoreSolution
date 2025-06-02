namespace MySuperStore.Entities.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }

    public decimal Price { get; set; }          // 💰 Цена
    public string ImageUrl { get; set; }        // 🖼️ Ссылка на изображение

    public ICollection<OrderProduct> OrderProducts { get; set; }
}
