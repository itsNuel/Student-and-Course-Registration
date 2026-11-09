using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using myproject.Data;
using Microsoft.AspNetCore.Mvc;
using myproject.Models;

namespace myproject.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly PasswordHasher<UserAccount> _passwordHasher = new();

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    [AllowAnonymous]
    public IActionResult SignUp()
    {
        return View();
    }



    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        string username = model.Username.Trim();
        string email = model.Email.Trim();

        bool usernameExists = await _context.UserAccounts
            .AnyAsync(user => user.Username == username);

        if (usernameExists)
        {
            ModelState.AddModelError(
                nameof(model.Username),
                "This username is already registered.");
            return View(model);
        }

        bool emailExists = await _context.UserAccounts
            .AnyAsync(user => user.Email == email);

        if (emailExists)
        {
            ModelState.AddModelError(
                nameof(model.Email),
                "This email address is already registered.");
            return View(model);
        }

        var user = new UserAccount
        {
            Username = username,
            Email = email
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

        _context.UserAccounts.Add(user);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Your account has been created. Please log in.";

        return RedirectToAction(nameof(Login));
    }


    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }



    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        LoginViewModel model,
        string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        string username = model.Username.Trim();

        var user = await _context.UserAccounts
            .FirstOrDefaultAsync(account => account.Username == username);

        if (user == null ||
            _passwordHasher.VerifyHashedPassword(
                user, user.PasswordHash, model.Password)
            == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(model);
        }

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Email, user.Email)
    };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);



        await HttpContext.SignInAsync(
     CookieAuthenticationDefaults.AuthenticationScheme,
     principal,
     new AuthenticationProperties
     {
         IsPersistent = model.RememberMe,
         ExpiresUtc = model.RememberMe
             ? DateTimeOffset.UtcNow.AddDays(14)
             : null
     });



        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }


    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction(nameof(Login));
    }


    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }
}