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
    public class ConexionesController : Controller
    {
        public class UserService
        {
            private readonly IConfiguration _configuration;

            public UserService(IConfiguration configuration)
            {
                _configuration = configuration;
            }

        }

    }
}
