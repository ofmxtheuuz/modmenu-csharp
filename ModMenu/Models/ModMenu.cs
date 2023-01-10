namespace ModMenu.Models;

public class ModMenu
{
    public int ModMenuId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Token { get; set; }
    public string FileName { get; set; }
}