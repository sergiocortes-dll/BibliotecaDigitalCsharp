using BibliotecaDigital.Data;
using BibliotecaDigital.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaDigital.Controllers;

public class UsersController : Controller
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var users = _context.Users.ToList();
        return View(users);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(User user)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            if (_context.Users.Any(u => u.DNI == user.DNI))
            {
                ModelState.AddModelError("DNI", "El DNI ya está registrado.");
                return View(user);
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Usuario registrado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Error al registrar usuario: " + ex.Message);
            return View(user);
        }
    }
}