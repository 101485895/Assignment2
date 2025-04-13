using System.ComponentModel.DataAnnotations;

namespace assgnment.Models;

public class Category
{
    public int categoryId { get; set; }
    
    [Required]
    public string categoryName { get; set; }
    
    public string? categoryDescription { get; set; }
    
    // One - to - many relationship with Product
    
    public List<Product> products { get; set; }
}