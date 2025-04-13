using System.ComponentModel.DataAnnotations;

namespace assgnment.Models;

public class Product
{
    public int productId { get; set; }
    
    [Required]
    public string productName { get; set; }
    
    public string? productDescription { get; set; }

    public double productPrice { get; set; }
    
    public int productQuantity { get; set; }
    
    public int stockThreshold {get; set;}
    
    // Foreign key for product
    public int CategoryId { get; set; }
    public Category  category { get; set; }
    
    // Many - to - many  relationship with orders
    public List<Order> Orders { get; set; }
}