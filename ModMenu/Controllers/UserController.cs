using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModMenu.Infra;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ModMenu.Controllers;

[Authorize(Roles = "Member")]
public class UserController : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly AppDbContext context;
    private IHostingEnvironment Environment;

    public UserController(AppDbContext context, IHostingEnvironment environment, UserManager<IdentityUser> userManager)
    {
        this.context = context;
        Environment = environment;
        this.userManager = userManager;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> Index()
    {
        var menus = await context.UserModMenus
            .Include(x => x.User)
            .Include(x => x.ModMenu)
            .Where(x => x.User.UserName == User.Identity.Name)
            .ToListAsync();
        return View(menus);
    }
    
    [Authorize]
    [HttpGet("/download/4671GDF46387924DASD6189743DSA689416437DAFSDDSAS892GFA32146FHJ87946M")]
    public async Task<FileResult> Download(string __id)
    {
        var id = User.Claims.FirstOrDefault(x => x.Type == "ID").Value;
        var usermods = await context.UserModMenus
            .Include(x => x.ModMenu)
            .Include(x => x.User)
            .Where(x => x.User.Id == id)
            .ToListAsync();
        var menu = usermods.FirstOrDefault(x => x.ModMenu.Token == __id);

        string path = Path.Combine(this.Environment.WebRootPath, "mods/") + menu.ModMenu.FileName;
        byte[] bytes = System.IO.File.ReadAllBytes(path);

        return File(bytes, "application/zip", menu.ModMenu.FileName);
    }
    
    [Authorize]
    public IActionResult ActiveKey()
    {
        return View();
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> KeyService(string __key)
    {
        var key = await context.Keys.Include(x => x.ModMenu).FirstOrDefaultAsync(x => x.Content == __key);
        if(key == null) return RedirectToAction("ActiveKey");
        var id = User.Claims.FirstOrDefault(x => x.Type == "ID").Value;
        var user = await userManager.FindByIdAsync(id);
        var mod = await context.ModMenus.FirstOrDefaultAsync(x => x.ModMenuId == key.ModMenu.ModMenuId);

        key.Valid = false;
        await context.UserModMenus.AddAsync(new()
        {
            ModMenu = mod,
            User = user
        });

        await context.SaveChangesAsync();
        
        return RedirectToAction("Index");
    }
}