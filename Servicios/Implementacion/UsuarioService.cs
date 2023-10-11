using Microsoft.EntityFrameworkCore;
using AppClientesMexaba.Models;
using AppClientesMexaba.Servicios.Contrato;
using AppClientesMexaba.Exceptions;

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
            var usuario_encontrado = await _DbContext.Tcausr
                .Where(u => u.nombre == nombre && u.pwd == pwd)
                .FirstOrDefaultAsync();

            if (usuario_encontrado == null)
            {
                // Aquí podrías lanzar una excepción o devolver un objeto que represente la falta de usuario.
                // Puedes personalizar según tus necesidades y políticas de manejo de errores.
                throw new UsuarioNoEncontradoException("El usuario no se encontró.");
            }

            return usuario_encontrado;
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
