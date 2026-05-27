using Microsoft.AspNetCore.Mvc;
using WhirlpoolPromptWeb.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using WhirlpoolPromptWeb.Filters;


namespace WhirlpoolPromptWeb.Controllers;

[Route("[controller]")]
public class LoginController : Controller
{

    private readonly IAuthenticatorService _service;

    public LoginController(IAuthenticatorService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid) return View(model);

        model.Email = model.Email.Trim();
        model.Password = model.Password.Trim();


        var userSession = await UserAuthentication(model.Email, model.Password);

        if (userSession != null)
        {
            SetUserSession(userSession);
            return RedirectToAction("Check", "Login");
        }

        // si no se encuentra en la base de datos
        ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");

        return View(model);

    }

    private void SetUserSession(UserSession user)
    {
        HttpContext.Session.SetInt32("UserId", user.id_usuario);
        HttpContext.Session.SetString("Name", user.nombre);
        HttpContext.Session.SetString("ProfileAddr", user.imagen_perfil);
        HttpContext.Session.SetInt32("Coins", user.saldo_total);
    }


    private async Task<UserSession?> UserAuthentication(string email, string password)
    {
        List<UserSession> userAuthentication = await _service.AuthenticateUserAPI(email, password);
        if (userAuthentication == null || userAuthentication.Count == 0) return null;

        return userAuthentication[0];

    }

    [HttpGet("check")]
    [RequireSession]
    public IActionResult Check()
    {

        var model = new UserSession
        {
            id_usuario = HttpContext.Session.GetInt32("UserId") ?? 0,
            nombre = HttpContext.Session.GetString("Name") ?? string.Empty,
            imagen_perfil = HttpContext.Session.GetString("ProfileAddr") ?? string.Empty,
            saldo_total = HttpContext.Session.GetInt32("Coins") ?? 0
        };


        return View(model);
    }


}