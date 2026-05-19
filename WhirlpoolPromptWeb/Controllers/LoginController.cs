using Microsoft.AspNetCore.Mvc;
using WhirlpoolPromptWeb.Models;


namespace WhirlpoolPromptWeb.Controllers;

[Route("[controller]")]
public class LoginController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginModel model)
    {
        if (!ModelState.IsValid) return View(model);

        bool userIsValid = UserAuthentication(model.User, model.Password);

        if (userIsValid)
        {
            return RedirectToAction("Index", "Home");
        }

        // agregar error si no se encuentra en la base de datos
        ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");

        return View(model);


    }

    private bool UserAuthentication(string user, string password)
    {
        // logica de autenticacion
        return true;
    }

    [HttpGet("check")]
    public IActionResult Check()
    {
        return View();
    }

}