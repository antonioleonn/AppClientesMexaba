using Microsoft.EntityFrameworkCore;
using AppClientesMexaba.Models;

namespace AppClientesMexaba.Servicios.Contrato
{
    public interface IUsuarioService
    {
        //SE UTILIZA TASK PARA TRABAJAR DE FORMA ASINCRONA
        Task<tcausr> GetTcausr(string nombre, string pwd);

        /*EJEMPLO DE METODO PARA PROYECTO QUE SI NECESITEN
         * REGISTRAR USUARIOS EN TIEMPO REAL EN LA BASE DE DATOS
         * 
        Task<Vusuario> SaveUsuario(string nombre, string pwd);
        FINALIZA METODO DE EJEMPLO */

    }
}
