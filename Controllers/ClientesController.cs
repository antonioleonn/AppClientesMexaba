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
using Microsoft.EntityFrameworkCore;
using AppClientesMexaba.Servicios;

namespace AppClientesMexaba.Controllers
{
    public class ClientesController : Controller
    {
        private readonly D_Cxccli _clientesData;
        private readonly IConfiguration _configuration;
        private readonly ClContext _dbContext;
        private readonly ITwilioService _twilioService;
        private readonly IClienteService _clienteService;
        private readonly UserService _userService;
        private readonly ITcausrService _tcausrService;

        public ClientesController(D_Cxccli clientesData, IConfiguration configuration, ClContext dbContext, ITwilioService twilioService, IClienteService clienteService, UserService userService, ITcausrService tcausrService)
        {
            _clientesData = clientesData;
            _configuration = configuration;
            _dbContext = dbContext;
            _twilioService = twilioService;
            _clienteService = clienteService;
            _userService = userService;
            _tcausrService = tcausrService;
        }

        //Metodo para listat clientes actuales de SQLSERVER
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

        //Metodo para renderizar a la vista agregar
        public IActionResult Agregar()
        {
            return View();
        }


        //Metodo para codigos postales
        [HttpPost]
        public IActionResult ObtenerEstadoPorCodigoPostal(string codigoPostal)
        {
            var estado = _dbContext.CodigosPostales
                .Where(cp => cp.c_codigopostal == codigoPostal)
                .Select(cp => cp.c_estado)
                .FirstOrDefault();

            return Json(estado);
        }

        //Metodo para obtener ciudades (pendiente)
        public IActionResult ObtenerCiudades(string term)
        {
            // Lógica para obtener la lista de ciudades que coinciden con 'term'
            var ciudades = _dbContext.Comestados
                .Where(c => c.ciudad.Contains(term))
                .Select(c => c.ciudad)
                .ToList();

            return Json(ciudades);
        }

        //Metodo para verificacion con Twilio
        [HttpPost]
        public IActionResult EnviarCodigoVerificacion(string numeroCelular)
        {
            // Generar un código de verificación (puedes usar una biblioteca para esto)
            string codigoVerificacion = GenerarCodigoVerificacion();

            // Almacenar el código de verificación en algún lugar (BD, memoria, etc.) para su posterior validación

            // Enviar el código de verificación al número de teléfono utilizando Twilio
            _twilioService.EnviarMensaje(numeroCelular, $"Su código de verificación es: {codigoVerificacion}");

            // Devolver una respuesta (puede ser un JSON que indique que el mensaje se envió correctamente)
            return Json(new { mensaje = "Código de verificación enviado correctamente" });
        }

        private string GenerarCodigoVerificacion()
        {
            // Lógica para generar un código de verificación (números aleatorios, etc.)
            return "123456"; // ¡Debes generar uno dinámicamente!
        }


        [HttpPost]
        public IActionResult CrearCliente(cxccli nuevoCliente)
        {
            string ciaVentas = ObtenerCiaVentasDelUsuario(); // Obtén el valor de 'cia_ventas'

            try
            {
                // Obtener la cadena de conexión correspondiente
                string connectionString = _userService.ObtenerConnectionStringDesdeCiaVentas(ciaVentas);

                // Utilizar Entity Framework u otro método para realizar la inserción
                var options = new DbContextOptionsBuilder<BaseDbContext>()
                    .UseSqlServer(connectionString)
                    .Options;

                using (var dbContext = new BaseDbContext(options))
                {
                    // Lógica para insertar en la base de datos
                    _clientesData.InsertarClienteCompleto(nuevoCliente, ciaVentas, dbContext, _tcausrService);
                }

                // Retorna un resultado exitoso después de la inserción
                return Ok("Cliente creado con éxito");
            }
            catch (Exception ex)
            {
                // Manejar errores
                return StatusCode(500, "Error al crear el cliente: " + ex.Message);
            }
        }




        private string ObtenerCiaVentasDelUsuario()
        {
            // Verificar si el usuario está autenticado
            if (User.Identity.IsAuthenticated)
            {
                // Obtener el nombre de la compañía de ventas del usuario autenticado
                // Esto es solo un ejemplo; ajusta la lógica según tu implementación
                string ciaVentas = User.FindFirst("cia_ventas")?.Value;

                // Si encontramos el valor, devolverlo
                if (!string.IsNullOrEmpty(ciaVentas))
                {
                    return ciaVentas;
                }
            }


            // Valor predeterminado si no encontramos la información del usuario
            return "valor_predeterminado";
        }



    }
}
