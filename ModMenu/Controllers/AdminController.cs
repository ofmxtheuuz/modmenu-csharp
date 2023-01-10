using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModMenu.Http.DTO;
using ModMenu.Http.Response;
using ModMenu.Infra;

namespace ModMenu.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly AppDbContext context;

    public AdminController(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<IActionResult> GenerateKey()
    {
        var mods = await context.ModMenus.ToListAsync();
        return View(mods);
    }
    
    [HttpPost("key")]
    public async Task<string> GenerateKeyService(string modmenu)
    {
        var menu = await context.ModMenus.FirstOrDefaultAsync(x => x.Token == modmenu);
        
        Random rand = new Random();
        
        string max = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJQLMNOPQRSTUVWXYZ1234567890";
        var str = ""; 

        for (int i = 0; i < 42; i++)
        {
            str += max[(int)Math.Floor((decimal)rand.Next(0, max.Length))];
        }

        str += menu.Title.ToUpper();

        await context.Keys.AddAsync(new()
        {
            Content = str,
            ModMenu = menu,
            Valid = true
        });
        await context.SaveChangesAsync();
        
        return str;
    }


    [HttpGet("admin/mods")]
    public async Task<IActionResult> Mods()
    {
        return View(await context.ModMenus.ToListAsync());
    }

    [HttpGet("admin/mods/create")]
    public IActionResult CreateMod()
    {
        return View();
    }

    [HttpGet("admin/mods/edit")]
    public async Task<IActionResult> EditMods(string token)
    {
        return View(new EditModsResponse()
        {
            ModMenu = await context.ModMenus.FirstOrDefaultAsync(x => x.Token == token)
        });
    }

    [HttpPost("admin/mods/exclude")]
    public async Task<IActionResult> ExcludeMod(string __Id)
    {
        context.Remove(await context.ModMenus.FirstOrDefaultAsync(x => x.Token == __Id));
        await context.SaveChangesAsync();
        return RedirectToAction("Mods");
    }

    [HttpPost]
    public async Task<IActionResult> EditModsService(EditModMenuDTO dto)
    {
        var mod = await context.ModMenus.FirstOrDefaultAsync(x => x.Token == dto.Token);
        mod.Title = dto.Title;
        mod.Description = dto.Description;
        mod.Price = dto.Price;
        mod.FileName = dto.FileName;

        await context.SaveChangesAsync();
        return RedirectToAction("Mods");
    }

    [HttpPost]
    public async Task<IActionResult> CreateModMenu(EditModMenuDTO dto)
    {
        var modmenu = new Models.ModMenu()
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            FileName = dto.FileName
        };

        await context.ModMenus.AddAsync(modmenu);

        modmenu.Token = this.GetSHA256(modmenu.ModMenuId.ToString());

        await context.SaveChangesAsync();

        return RedirectToAction("Mods");

    }

    private string GetSHA256(string text)
    {
        byte[] bytes = Encoding.Unicode.GetBytes(text);
        SHA256Managed hashstring = new SHA256Managed();
        byte[] hash = hashstring.ComputeHash(bytes);
        string hashString = string.Empty;
        foreach (byte x in hash)
        {
            hashString += String.Format("{0:x2}", x);
        }
        return hashString;
    }
}