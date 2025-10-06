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
    public async Task<IActionResult> Create(User user)
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
            await _context.SaveChangesAsync();

            ViewBag.SuccessMessage = "Usuario registrado correctamente.";

            return View();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Error al registrar usuario: " + ex.Message);
            return View(user);
        }
    }


    public IActionResult Edit(int userId)
    {
        var user = _context.Users.Find(userId);
        return View(user);
    }

    [HttpPost]
    public IActionResult Edit(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int userId)
    {
        var user = _context.Users.Find(userId);
        return View(user);
    }

    [HttpPost]
    public IActionResult Delete(User user)
    {
        _context.Users.Remove(user);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}