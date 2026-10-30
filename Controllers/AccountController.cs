using Microsoft.AspNetCore.Mvc;
using myproject.Models;

namespace myproject.Controllers;

public class AccountController : Controller
{
    // Displays the login form.
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // Processes the submitted login form.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Classroom demo only; do not hard-code credentials in a real application.
        bool validDemoCredentials =
            model.Username == "student" &&
            model.Password == "Demo123!";

        if (!validDemoCredentials)
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(model);
        }

        // This demo session value is not real cookie authentication.
        HttpContext.Session.SetString("DemoUser", model.Username);

        return RedirectToAction("Index", "Home");
    }

    // Ends the classroom demo session.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("DemoUser");

        return RedirectToAction(nameof(Login));
    }
}