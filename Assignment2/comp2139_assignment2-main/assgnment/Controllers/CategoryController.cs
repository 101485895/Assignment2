using assgnment.Data;
using assgnment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace assgnment.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var categories = _context.Categories.ToList();
        return View(categories);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(category);
    }

    [HttpGet]
    public IActionResult Details(int categoryId)
    {
        // Show products in the category page
        var category = _context.Categories
            .Include(c => c.products)
            .FirstOrDefault(c => c.categoryId == categoryId);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    [HttpGet]
    public IActionResult Edit(int categoryId)
    {
        var category = _context.Categories.Find(categoryId);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    private bool CategoryExists(int categoryId)
    {
        return _context.Categories.Any(c => c.categoryId == categoryId);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit
        (int id, [Bind("categoryId,categoryName,categoryDescription")] Category category)
    {
        if (id != category.categoryId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(category);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.categoryId))
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
        return View(category);
    }

    [HttpGet]
    public IActionResult Delete(int categoryId)
    {
        var category = _context.Categories.FirstOrDefault(c => c.categoryId == categoryId);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int categoryId)
    {
        var category = _context.Categories.Find(categoryId);
        if (category != null)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return NotFound();
    }
}