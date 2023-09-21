using Microsoft.EntityFrameworkCore;
using AppClientesMexaba.Models;
using AppClientesMexaba.Servicios.Contrato;

namespace AppClientesMexaba.Servicios.Implementacion
{
    public class UsuarioService : IUsuarioService
    {   

        private readonly ClContext _DbContext;

        public UsuarioService(ClContext DbContext)
        {
            _DbContext = DbContext;
        }

        //INICIA METODO PARA ENCONTRAR USUARIO
        public async Task<tcausr> GetTcausr(string nombre, string pwd)
        {
            if (_DbContext.Tcausr != null)
            {
                tcausr usuario_encontrado = await _DbContext.Tcausr
                    .Where(u => u.nombre == nombre && u.pwd == pwd)
                    .FirstOrDefaultAsync();
                return usuario_encontrado;
            }
            else
            {
                // Manejar la situación en la que _DbContext.Tcausr es nulo, por ejemplo, lanzando una excepción o devolviendo un valor predeterminado.
                return null; // O manejar el error de acuerdo a tus necesidades.
            }
        }
        //FINALIZA METODO PARA ENCONTRAR USUARIO

        /*INICIA METODO DE EJEMPLO PARA PROYECTOS QUE SI NECESITEN
         REGISTRAR USUARIOS EN TIEMPO REAL EN LA BASE DE DATOS
         
       
        public Task <Tcausr>SaveUsuario(Vusuario modelo)
        {
            _DbContext.Tcausr.Add(modelo);
            await _DbContext.SaveChangesAsync();
            return modelo;
        }
        FINALIZA METODO DE EJEMPLO */
    }
}
