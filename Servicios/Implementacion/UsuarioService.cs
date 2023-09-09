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
        public async Task<Vusuario> GetVusuario(string nombre, string pwd)
        {
            Vusuario usuario_encontrado = await _DbContext.Vusuarios.Where(u => u.nombre == nombre && u.pwd == pwd).FirstOrDefaultAsync();
            return usuario_encontrado;
        }
        //FINALIZA METODO PARA ENCONTRAR USUARIO

        /*INICIA METODO DE EJEMPLO PARA PROYECTOS QUE SI NECESITEN
         REGISTRAR USUARIOS EN TIEMPO REAL EN LA BASE DE DATOS
         
       
        public Task <Vusuario>SaveUsuario(Vusuario modelo)
        {
            _DbContext.Vusuarios.Add(modelo);
            await _DbContext.SaveChangesAsync();
            return modelo;
        }
        FINALIZA METODO DE EJEMPLO */
    }
}
