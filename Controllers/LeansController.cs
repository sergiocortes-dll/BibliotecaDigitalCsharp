using BibliotecaDigital.Data;
using BibliotecaDigital.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BibliotecaDigital.Controllers;

public class LeansController : Controller
{
    private readonly AppDbContext _context;

    public LeansController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var leans = _context.Leans
            .Include(p => p.User)
            .Include(p => p.Book)
            .ToList();

        return View(leans);
    }

    public IActionResult Create()
    {
        var users = _context.Users.ToList();
        var books = _context.Books.Where(l => l.Available > 0).ToList();
        ViewBag.Users = new SelectList(users, "Id", "Name");
        ViewBag.Books = new SelectList(books, "Id", "Title");
        return View();
    }

    [HttpPost]
    public IActionResult Create(Lean lean)
    {
        try
        {
            var book = _context.Books.Find(lean.BookId);

            if (book == null)
            {
                ModelState.AddModelError("BookId", "Libro no encontrado.");
            }
            else if (book.Available <= 0)
            {
                ModelState.AddModelError("BookId", "No hay ejemplares disponibles.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Users = new SelectList(_context.Users.ToList(), "Id", "Name", lean.UserId);
                ViewBag.Books = new SelectList(_context.Books.Where(b => b.Available > 0).ToList(), "Id", "Title", lean.BookId);
                return View(lean);
            }

            book.Available--;
            _context.Books.Update(book);

            _context.Leans.Add(lean);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Error al registrar el préstamo: " + ex.Message);

            ViewBag.Users = new SelectList(_context.Users.ToList(), "Id", "Name", lean.UserId);
            ViewBag.Books = new SelectList(_context.Books.Where(b => b.Available > 0).ToList(), "Id", "Title", lean.BookId);
            return View(lean);
        }
    }
    
    public IActionResult UserHistory(int userId)
    {
        var leans = _context.Leans
            .Include(p => p.Book)
            .Include(p => p.User)
            .Where(p => p.UserId == userId)
            .ToList();

        return View("History", leans);
    }

    public IActionResult BookHistory(int bookId)
    {
        var leans = _context.Leans
            .Include(p => p.User)
            .Include(p => p.Book)
            .Where(p => p.BookId == bookId)
            .ToList();
        return View("History", leans);
    }

    public IActionResult ReturnBook(int id)
    {
        try
        {
            var lean = _context.Leans
                .Include(p => p.Book)
                .FirstOrDefault(p => p.Id == id);

            if (lean != null)
            {
                lean.Book.Available++;
                _context.Leans.Remove(lean);
                _context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error al devolver el libro: " + ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}