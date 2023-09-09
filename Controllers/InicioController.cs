using Microsoft.AspNetCore.Mvc;

using AppClientesMexaba.Models;
using AppClientesMexaba.Recursos;
using AppClientesMexaba.Servicios.Contrato;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace AppClientesMexaba.Controllers
{
    public class InicioController : Controller
    {   
        private readonly IUsuarioService _usuarioServicio;

        public InicioController(IUsuarioService usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }


        /*EN ESTO PROYECTO NO SE UTILIZA UN METODO PARA REGISTRAR USUARIO
        DEBIDO A QUE LOS USUARIOS SON CONTRALADOS POR SU BASE DE DATOS DE ERP
        SE CREA METODO COMO EJEMPLO PARA FUTUROS PROYECTOS EN DONDE SI
        SE NECESITA CREAR USUARIO

        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Registrarse(Usuario modelo)
        {
            modelo.Clave = Utilidades.EncriptarClave(modelo.Clave);
            
            Vusuario usuario_creado = await _usuarioServicio.SaveUsuario(modelo);

            if(usuario_creado.idUsuario > 0)
                return RedirectToAction("IniciarSesión","Inicio");

            ViewData["Mensaje"] = "No se pudo crear el usuario";            
            return View();
        }

         FINALIZA EJEMPLO DE METODO PARA CREAR USUARIO*/



        //METODO GET PARA INICIAR SESIÓN
        public IActionResult IniciarSesion()
        {
            return View();
        }

        //METODO POST PARA INICIAR SESIÓN
        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string nombre, string pwd)
        {
            Vusuario usuario_encontrado = await _usuarioServicio.GetVusuario(nombre, pwd);

            if(usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            //CONFIGURAR AUTENTICACIÓN DEL USUARIO
            List <Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,usuario_encontrado.nombre_lar)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            return RedirectToAction("Index","Home");
        }
    }
}
