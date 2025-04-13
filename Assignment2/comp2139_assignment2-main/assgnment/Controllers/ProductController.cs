using assgnment.Data;
using assgnment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
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
    public IActionResult IndexAll(decimal? minPrice, decimal? maxPrice)
    {
        var products = _context.Products.AsQueryable();
        if (minPrice.HasValue)
        {
            products = products.Where(p => p.productPrice >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            products = products.Where(p => p.productPrice <= maxPrice.Value);
        }
        return View(products.ToList());
    }
    [HttpGet]
    public IActionResult Index(int? categoryId)
    {
        // To Access category name
        var category = _context.Categories.Find(categoryId);
        if (category == null)
        {
            return NotFound();
        }
        // List products that are in categories
        var products = _context.Products
            .Include(p => p.category)
            .Where(p => p.CategoryId == categoryId)
            .ToList();
        ViewBag.CategoryId = categoryId;
        ViewBag.CategoryName = category.categoryName;
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
            category = category
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
            // Add the Product to the context
            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index", new { categoryId = product.CategoryId, 
                category = _context.Categories.FirstOrDefault(c => c.categoryId == product.CategoryId) });
        }

        return View(product);
    }

    [HttpGet]
    public IActionResult Details(int productId)
    {
        var product = _context.Products
            .Include(p => p.category)
            .FirstOrDefault(p => p.productId == productId);
        if (product == null)
        {
            return NotFound();
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
            return RedirectToAction("Index", product);
        }
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int productId)
    {
        var product = _context.Products.Find(productId);
        if (product != null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index", new { categoryId = product.CategoryId });
        }
        return NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> Search(string searchString, decimal? minPrice, decimal? maxPrice)
    {
        var productsQuery = _context.Products.AsQueryable();

        // If there's a search string, filter by it
        if (!string.IsNullOrEmpty(searchString))
        {
            searchString = searchString.ToLower();
            productsQuery = productsQuery.Where(p => p.productName.Contains(searchString) || 
                                                     p.productDescription.Contains(searchString));
        }

        // If there's a min price, filter by it
        if (minPrice.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.productPrice >= minPrice);
        }

        // If there's a max price, filter by it
        if (maxPrice.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.productPrice <= maxPrice);
        }

        // Execute the query and return the results
        var products = await productsQuery  
            .Select(p => new 
            { 
                p.productName, 
                p.productDescription, 
                p.productPrice, 
                p.productQuantity 
            })
            .ToListAsync();
    
        return Json(products);  // Return results as JSON
    }
    
    [HttpPost]
    public async Task<IActionResult> AddProductAjax([FromForm] Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return Json(new
        {
            product.productId,
            product.productName,
            product.productDescription,
            product.productPrice,
            product.productQuantity
        });
    }
}