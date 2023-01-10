using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModMenu.Infra;
using ModMenu.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ModMenu.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly AppDbContext context;
    
    public HomeController(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        this.context = context;
        this.userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        return View(await context.ModMenus.ToListAsync());
    }
    
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    


}