using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AppClientesMexaba.Models;
using AppClientesMexaba.Recursos;
using AppClientesMexaba.Servicios.Contrato;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using AppClientesMexaba.Data;
using System.Linq;
using System.Threading.Tasks;
using Humanizer.Configuration;
using Microsoft.VisualBasic;
using System.Data;
using System.Numerics;

namespace AppClientesMexaba.Controllers
{
    public class ClientesController : Controller
    {
        private readonly D_Cxccli _clientesData;
        private readonly IConfiguration _configuration;

        public ClientesController(D_Cxccli clientesData, IConfiguration configuration)
        {
            _clientesData = clientesData;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 20)
        {
            // Calcula el índice de inicio en función de la página y el tamaño de la página
            var startIndex = (page - 1) * pageSize;

            // Utiliza tu capa de datos para obtener los datos de SQL Server
            var data = _clientesData.Listar(); // Obtén todos los datos sin paginación

            var totalRecords = data.Count; // Obtén el total de registros

            var pagedData = data
                .Skip(startIndex)
                .Take(pageSize)
                .ToList();

            var viewModel = new List<cxccli>(pagedData);

            return View(viewModel);
        }




    }
}
