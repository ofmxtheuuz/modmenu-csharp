using Microsoft.AspNetCore.Identity;

namespace ModMenu.Models;

public class Key
{
    public int KeyId { get; set; }
    public string Content { get; set; }
    public ModMenu ModMenu { get; set; }
    public bool Valid { get; set; } = true;
}