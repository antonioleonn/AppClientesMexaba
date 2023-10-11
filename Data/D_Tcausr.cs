using AppClientesMexaba.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace AppClientesMexaba.Data
{
    public class D_Tcausr
    {
        private readonly IConfiguration _configuration;

        public D_Tcausr(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //LISTADO DE USUARIOS
        public List<tcausr> Listar()
        {
            List<tcausr> lista = new List<tcausr>();

            using (SqlConnection oconexion = new SqlConnection(_configuration.GetConnectionString("CadenaAut")))
            {
                try
                {
                    oconexion.Open();

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT nombre, nom_cto, pwd, nombre_lar, puesto, cia_ventas FROM tcausr");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new tcausr()
                            {
                                nombre = dr["nombre"].ToString(),
                                nom_cto = dr["nom_cto"].ToString(),
                                pwd = dr["pwd"].ToString(),
                                nombre_lar = dr["nombre_lar"].ToString(),
                                puesto = dr["puesto"].ToString(),
                                cia_ventas = dr["cia_ventas"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return lista;
        }
    }
}
