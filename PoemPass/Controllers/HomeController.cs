using Microsoft.AspNetCore.Mvc;
using PoemPass.Models;
using PoemPass.Services;

namespace PoemPass.Controllers;

public class HomeController : Controller
{
    private PoemPassGenerator _generator;
    
    public HomeController(PoemPassGenerator generator) => _generator = generator;

    [HttpGet]
    public IActionResult Index() => View();
    
    [HttpGet]
    public IActionResult Info() => View();

    [HttpPost]
    public UserResponse Generate([FromBody]AcceptDataModel model)
    {
        if (model.Length < 1 || model.Length > 256)
        {
            Models.UserResponse errorResponse = new UserResponse() { Error = "Entered value can't be less than 1 and higher than 256!" };
            return errorResponse;
        }
        
        var response = _generator.GeneratePoemPass(model);
        return response;
    }
}