using BibliotecaDigital.Data;
using BibliotecaDigital.Models;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaDigital.Controllers;

public class BooksController : Controller
{
    private readonly AppDbContext _context;

    public BooksController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var books = _context.Books.ToList();
        return View(books);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Book book)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }

            if (_context.Books.Any(u => u.Code == book.Code))
            {
                ModelState.AddModelError("Code", "Ese Código ya está registrado.");
                return View(book);
            }

            _context.Books.Add(book);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Libro registrado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Error al registrar libro: " + ex.Message);
            return View(book);
        }
    }

    public IActionResult Edit(int id)
    {
        var book = _context.Books.Find(id);
        return View(book);
    }
}