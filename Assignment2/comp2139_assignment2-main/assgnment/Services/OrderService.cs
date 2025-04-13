using assgnment.Data;
using assgnment.Models;
using Microsoft.Extensions.Logging;

public class OrderService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OrderService> _logger;

    public OrderService(ApplicationDbContext context, ILogger<OrderService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public void ProcessOrder(Order order)
    {
        try
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while saving order.");
            throw;
        }
    }
}
