using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ModMenu.Controllers;

public class AuthenticationController : Controller
{
    
    private readonly UserManager<IdentityUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly SignInManager<IdentityUser> signInManager;

    public AuthenticationController(SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    {
        this.signInManager = signInManager;
        this.roleManager = roleManager;
        this.userManager = userManager;
    }

    public IActionResult Login()
    {
        /*await userManager.CreateAsync(new()
        {
            UserName = "Admin",
            Email = "admin@gmail.com"
        }, "Admin@2023");
        await roleManager.CreateAsync(new()
        {
            Name = "Admin"
        });
        var user = await userManager.FindByEmailAsync("admin@gmail.com");
        await userManager.AddToRoleAsync(user, "Admin");*/
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> LoginService(string username, string password)
    {
        var result = await signInManager.PasswordSignInAsync(username, password, true, false);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "User");
        }
        return RedirectToAction("Login");
    }
    
    
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> RegisterService(string username, string email, string password)
    {
        var result = await userManager.CreateAsync(new()
        {
            UserName  = username,
            Email = email
        }, password);
        if (result.Succeeded)
        {
            var user = await userManager.FindByEmailAsync(email);
            await roleManager.CreateAsync(new()
            {
                Name = "Member"
            });
            await userManager.AddToRoleAsync(user, "Member");

            await userManager.AddClaimsAsync(user, new List<Claim>()
            {
                new Claim("ID", user.Id),
                new Claim("Email", user.Email),
                new Claim("Username", user.UserName),
            });
            return RedirectToAction("Login");
        }
        return RedirectToAction("Register");
    }


    [Authorize]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}