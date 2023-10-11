namespace AppClientesMexaba.Servicios
{
    public class UserService
    {

        private readonly IConfiguration _configuration;

        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ObtenerConnectionStringDesdeCiaVentas(string ciaVentas)
        {
            try
            {
                return _configuration.GetConnectionString(ciaVentas);
            }
            catch (Exception ex)
            {
                // Puedes registrar el error si es necesario
                Console.WriteLine($"Error al obtener la cadena de conexión: {ex.Message}");

                // Puedes lanzar una excepción específica o devolver un valor predeterminado
                throw new ArgumentException($"Compañía de ventas no reconocida: {ciaVentas}");
            }


        }




    }
}
