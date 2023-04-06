using Microsoft.AspNetCore.Mvc;
using PoemPass.Enums;
using PoemPass.Models;
using PoemPass.Services;

namespace PoemPass.Controllers;

public class HomeController : Controller
{
    private string info = @"Our service is designed to generate passwords that are easy to remember thanks to the 'poem' that is generated along with the password. When the password symbol is capital, then the name of the person is selected as word for poem. You can also generate a poem using your own password.<br><br>""Reverse mode"" is a mode where in the generated poem each letter of the password is at the end of each word. If the character starts with a capital letter, then the word will also begin with a capital letter, but its last letter remains small.";
    
    private Generator _generator;
    
    public HomeController(Generator generator)
    {
        _generator = generator;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Info()
    {
        return View("Info", info);
    }

    [HttpPost]
    public Response Generate([FromBody]AcceptDataModel model)
    {
        if (model.Length < 1 || model.Length > 256)
        {
            Models.Response errorResponse = new Response() { Error = "Entered value can't be less than 1 and higher than 256!" };
            return errorResponse;
        }
        var response = _generator.Generate(model);
        return response;
    }
}