using AppClientesMexaba.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Configuration;


namespace AppClientesMexaba.Data
{
    public class D_Cxccli
    {
        private readonly IConfiguration _configuration;

        public D_Cxccli(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //LISTADO DE CLIENTES Y CONEXION SQLSERVER VISTA VCLIENTES
        public List<cxccli> Listar()
        {
            List<cxccli> lista = new List<cxccli>();

            using (SqlConnection oconexion = new SqlConnection(_configuration.GetConnectionString("CadenaAut")))
            {
                try
                {
                    oconexion.Open();

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT cve, vendedor, nom, dir1, cd, est, pais, pos, tel2, seg_mer, con_pag, for_pgo, suc, status, rfc, mail1, usocfdi FROM cxccli");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new cxccli()
                            {
                                cve = dr["cve"].ToString(),
                                vendedor = dr["vendedor"].ToString(),
                                nom = dr["nom"].ToString(),
                                dir1 = dr["dir1"].ToString(),
                                cd = dr["cd"].ToString(),
                                est = dr["est"].ToString(),
                                pais = dr["pais"].ToString(),
                                pos = dr["pos"].ToString(),
                                tel2 = dr["tel2"].ToString(),
                                seg_mer = dr["seg_mer"].ToString(),
                                con_pag = dr["con_pag"].ToString(),
                                for_pgo = dr["for_pgo"].ToString(),
                                suc = dr["suc"].ToString(),
                                status = dr["status"].ToString(),
                                rfc = dr["rfc"].ToString(),
                                mail1 = dr["mail1"].ToString(),
                                usocfdi = dr["usocfdi"].ToString(),
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
