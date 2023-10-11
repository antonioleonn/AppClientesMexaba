using AppClientesMexaba.Data;
using AppClientesMexaba.Models;
using AppClientesMexaba.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace AppClientesMexaba.Controllers
{
    public class CxccliController : Controller
    {

        private readonly ITcausrService _tcausrService;

        public CxccliController(ITcausrService tcausrService)
        {
            _tcausrService = tcausrService; // Corregir el nombre aquí
        }

        public IActionResult Index()
        {
            // Aquí obtienes una instancia de tcausr utilizando tu servicio
            tcausr instanciaTcausr = _tcausrService.ObtenerTcausrPorNombreCto("NombreDeEjemplo");

            // Hacer algo con instanciaTcausr, por ejemplo, pasarla a la vista
            return View(instanciaTcausr);
        }
    }
}

