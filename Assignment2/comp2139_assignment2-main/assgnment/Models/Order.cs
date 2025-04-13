using System.ComponentModel.DataAnnotations;

namespace assgnment.Models;

public class Order
{
    public int OrderId { get; set; }
    
    [Required]
    public string FirstName { get; set; }
   
    [Required]
    public string LastName { get; set; }
   
    [Required]
    public string Email { get; set; }
   
    [DataType(DataType.Date)]
    public DateTime OrderDate { get; set; }
    
    public double OrderTotal { get; set; }
    
    // Many to many relationship with products
    public List<Product> Products { get; set; }
}