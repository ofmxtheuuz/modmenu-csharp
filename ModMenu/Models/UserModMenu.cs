using Microsoft.AspNetCore.Identity;

namespace ModMenu.Models;

public class UserModMenu
{
    public int UserModMenuId { get; set; }
    public IdentityUser User { get; set; }
    public ModMenu ModMenu { get; set; }
}