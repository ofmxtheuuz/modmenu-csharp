using ModMenu.Http.DTO;

namespace ModMenu.Http.Response;

public class EditModsResponse
{
    public Models.ModMenu ModMenu { get; set; }
    public EditModMenuDTO dto { get; set; }
}