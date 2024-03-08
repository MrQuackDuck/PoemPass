namespace PoemPass.Models;

public class UserResponse
{
    public string Password { get; set; } = "";
    public string Poem { get; set; } = "";
    public string Error { get; set; } = "";
}