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
            SetUserSession(1);
            return RedirectToAction("Check", "Login");
        }

        // agregar error si no se encuentra en la base de datos
        ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");

        return View(model);


    }

    private bool SetUserSession(int userID)
    {

        UserSession user = getUserFromId(userID);

        HttpContext.Session.SetInt32("UserId", user.UserId);
        HttpContext.Session.SetString("Name", user.Name);
        HttpContext.Session.SetString("ProfileAddr", user.ProfileAddr);
        HttpContext.Session.SetInt32("Coins", user.Coins);

        return true;

    }

    private UserSession getUserFromId(int id)
    {
        UserSession user = new UserSession();

        /* TODO: Obtener el user de la base de datos */
        user.UserId = id;
        user.Name = "Yoshi";

        user.Coins = 67;

        user.ProfileAddr = GetProfileAddr(ProfilePhoto.yoshi);

        return user;
    }

    private string GetProfileAddr(ProfilePhoto profile)
    {
        switch (profile)
        {
            case ProfilePhoto.mario:
                return "mario-bros.png";
            default:
                return "mario-bros.png";
        }
    }

    private bool UserAuthentication(string user, string password)
    {
        // logica de autenticacion
        return true;
    }

    [HttpGet("check")]
    public IActionResult Check()
    {
        if (!IsSessionActive()) return RedirectToAction("Login");

        var model = new UserSession
        {
            UserId = HttpContext.Session.GetInt32("UserId") ?? 0,
            Name = HttpContext.Session.GetString("Name") ?? string.Empty,
            ProfileAddr = HttpContext.Session.GetString("ProfileAddr") ?? string.Empty,
            Coins = HttpContext.Session.GetInt32("Coins") ?? 0
        };


        return View(model);
    }

    private bool IsSessionActive() => HttpContext.Session.GetInt32("UserId") != null;

}