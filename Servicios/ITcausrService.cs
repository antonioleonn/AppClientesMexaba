using AppClientesMexaba.Models;

namespace AppClientesMexaba.Servicios
{
    public interface ITcausrService
    {
        tcausr ObtenerTcausrPorNombreCto(string nombreCto);
    }


    public class TcausrService : ITcausrService
    {
        private readonly BaseDbContext _dbContext;

        public TcausrService(BaseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public tcausr ObtenerTcausrPorNombreCto(string nombreCto)
        {
            if (string.IsNullOrEmpty(nombreCto))
            {
                // Manejar el caso cuando el nombreCto es nulo o vacío
                return null; // O lanzar una excepción, según la lógica de tu aplicación
            }

            try
            {
                // Consultar la base de datos utilizando Entity Framework u otra lógica de acceso a datos
                return _dbContext.Tcausr.FirstOrDefault(u => u.nom_cto == nombreCto);
            }
            catch (Exception ex)
            {
                // Manejar excepciones de acceso a datos
                Console.WriteLine($"Error al obtener el usuario: {ex.Message}");
                return null; // O lanzar una excepción, según la lógica de tu aplicación
            }
        }
    }
}
