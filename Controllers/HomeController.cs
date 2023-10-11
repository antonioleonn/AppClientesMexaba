using AppClientesMexaba.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AppClientesMexaba.Controllers
{

    //SE AGREGA LINEA DE CODIGO PARA QUE SOLO LOS USUARIOS AUTENTICADOS
    //PUEDAN INICIAR A HOMECONTROLLER
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;




        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string nombreUsuario = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                nombreUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["nombreUsaurio"] = nombreUsuario;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //SE GENERA MENU Y EVENTO DE CERRAR SESIÓN EN LA APLICACIÓN
        //SE TRABAJA DE FORMA ASINCRONA ES POR ESO EL TASK
        public async Task<IActionResult> CerrarSesion()
        {
            //Borramos autenticación
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //REDIRECCIONAR 
            return RedirectToAction("IniciarSesion", "Inicio");
        }
    }
}