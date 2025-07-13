using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace StudentApp.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    // GET: auth/login
    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }
    
    // POST: auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        // hardcoded credentials for demonstration purposes
        if (username == "admin" && password == "123456")
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var identity = new ClaimsIdentity(claims, "MyCookie");
            var principal = new ClaimsPrincipal(identity);
            
            await HttpContext.SignInAsync("MyCookie", principal);
            return RedirectToAction("Index", "Student");
        }

        ViewBag.Error = "Invalid username or password.";
        return View();
    }
    
    // POST: auth/logout
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("MyCookie");
        return RedirectToAction("Login");
    }
}