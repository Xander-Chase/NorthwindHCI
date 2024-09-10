using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NorthwindHCI.Models.NW;

namespace NorthwindHCI.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    NorthwindContext _context = new();

    // Property to bind the form input
    [BindProperty]
    public string CategoryName { get; set; } = string.Empty;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        // Fetch categories with more details from the database
        var categories = _context.Categories
            .Select(c => new Category
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description
            }).ToList();
        
        // Store categories in ViewData for rendering in the Razor page
        ViewData["Categories"] = categories;
    }

    // This method will be called on POST (form submission)
    public IActionResult OnPost()
    {
        if (!string.IsNullOrEmpty(CategoryName))
        {
            // Insert the new category
            InsertCategory(CategoryName);
        }

        // Reload the categories after the insertion
        OnGet();

        // Return the page to display the updated list
        return Page();
    }

    // Method to insert the category into the database
    int InsertCategory(string name)
    {
        var timeStart = DateTime.Now;

        Category category = new() { CategoryName = name };
        _context.Categories.Add(category);
        _context.SaveChanges();
        
        var timeEnd = DateTime.Now;
        Console.WriteLine($"Time taken: {timeEnd - timeStart}");

        return category.CategoryId;
    }
}
