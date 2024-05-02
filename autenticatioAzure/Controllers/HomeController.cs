using autenticatioAzure.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace autenticatioAzure.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            String mensaje = "";

            if (User.Identity.IsAuthenticated)
                mensaje = "El usuario se encuentra logueado";
            else
                mensaje = "El usuario NO se encuentra logueado";

            ViewBag.mensaje = mensaje;
            return View();
        }

        public IActionResult Privacy()
        {
            String mensaje = "";

            if (User.Identity.IsAuthenticated)
                mensaje = "El usuario se encuentra logueado";
            else
                mensaje = "El usuario NO se encuentra logueado";

            ViewBag.mensaje = mensaje;

            // Obtén la información del usuario
            var user = GetUserFromSomeSource();

            var profileModel = new UserProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return View(profileModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = Url.Action("Privacy", "Home") }, OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult Logout()
        {
            return SignOut(new AuthenticationProperties { RedirectUri = Url.Action("Index", "Home") },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        private UserProfileViewModel GetUserFromSomeSource()
        {
            // Obtener el objeto ClaimsPrincipal del usuario autenticado
            var user = HttpContext.User;

            // Crear un objeto User con la información de las claims
            var userProfile = new UserProfileViewModel
            {
                UserName = user.Identity.Name,
                Email = user.FindFirst(ClaimTypes.Surname)?.Value,
                FirstName = user.FindFirst(ClaimTypes.GivenName)?.Value,
                LastName = user.FindFirst(ClaimTypes.Surname)?.Value
                // Agregar más propiedades si es necesario
            };

            return userProfile;
        }
    }
}