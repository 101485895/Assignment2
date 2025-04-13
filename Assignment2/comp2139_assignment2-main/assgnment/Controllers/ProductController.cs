using assgnment.Data;
using assgnment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace assgnment.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        // List products that are in categories
        var products = _context.Products
            .Include(p => p.category)
            .ToList();
        return View(products);
    }

    [HttpGet]
    public IActionResult Create(int categoryId)
    {
        var category = _context.Categories.Find(categoryId);
        if (category == null)
        {
            return NotFound();
        }

        var product = new Product
        {
            CategoryId = categoryId,
            productName = "",
            productDescription = "",
            productPrice = 0,
            productQuantity = 0,
            stockThreshold = 0,
        };
        ViewBag.categoryId = categoryId;
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(product);
    }

    [HttpGet]
    public IActionResult Edit(int productId)
    {
        var product = _context.Products.Find(productId);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    private bool ProductExists(int productId)
    {
        return _context.Products.Any(p => p.productId == productId);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int productId,
        [Bind("productId,productName,productDescription,productPrice,productQuantity,stockThreshold,CategoryId")]
        Product product)
    {
        if (productId != product.productId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.productId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }
        return View(product);
    }

    [HttpGet]
    public IActionResult Delete(int productId)
    {
        var product = _context.Products.Find(productId);
        if (product != null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return NotFound();
    }
}