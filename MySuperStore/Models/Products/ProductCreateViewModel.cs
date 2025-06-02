using System.ComponentModel.DataAnnotations;

namespace MySuperStore.Models.Products;

public class ProductCreateViewModel
{
    [Required]
    public string Name { get; set; }

    [Required]
    [Range(0.01, 999999)]
    public decimal Price { get; set; }

    [Display(Name = "Image URL")]
    public string ImageUrl { get; set; }
}
