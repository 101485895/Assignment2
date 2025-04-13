using assgnment.Data;
using assgnment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace assgnment.Controllers;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;

    public OrderController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        // List orders along with related OrderProducts and Products
        var orders = _context.Orders
            .Include(o => o.Products)
            .ToList();
        return View(orders);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Products = _context.Products.ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Order order)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                Console.Error.WriteLine($"Db error: {ex.message}");
                ViewBag.ErrorMessage = "Could not save order. Please try again.";
                return View("Error");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return RedirectToAction("Error500", "Error");
            }
        }
        return View(order);
    }

    [HttpGet]
    public IActionResult Details(int orderId)
    {
        var order = _context.Orders
            .Include(o => o.Products)
            .FirstOrDefault(o => o.OrderId == orderId);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }

    [HttpGet]
    public IActionResult Edit(int orderId)
    {
        var order = _context.Orders.Find(orderId);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit
        (int orderId,[Bind("OrderId, FirstName, LastName, Email, OrderDate, OrderTotal")] Order order)
    {
        if (orderId != order.OrderId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(order);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.OrderId))
                {
                    return NotFound();
                }
                else
                {
                    ViewBag.ErrorMessage = "There was a conflict while updating the order. Please try again.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.message}");
                return RedirectToAction("Error500", "Error");
            }
        }
        return View(order);
    }

    private bool OrderExists(int orderId)
    {
        return _context.Orders.Any(e => e.OrderId == orderId);
    }

    [HttpGet]
    public IActionResult Delete(int orderId)
    {
        var order = _context.Orders.Find(orderId);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int orderId)
    {
        try
        {
            var order = _context.Orders.Find(orderId);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        catch (DbUpdateException ex)
        {
            Console.Error.WriteLine($"Delete error: {ex.Message}");
            ViewBag.ErrorMessage = "Unable to delete order. Please try again later.";
            return View("Error");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            return RedirectToAction("Error500", "Error");
        }
    }
}