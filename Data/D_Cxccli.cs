using AppClientesMexaba.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Collections.Generic;
using AppClientesMexaba.Servicios;
using AppClientesMexaba.Servicios.Implementacion;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Identity.Client;

namespace AppClientesMexaba.Data
{
    public class D_Cxccli
    {
        private readonly IConfiguration _configuration;
        private readonly BaseDbContext _dbContext;
        private readonly List<IClienteInserter> _clienteInserters;
        private readonly ITcausrService _tcausrService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public D_Cxccli(IConfiguration configuration, List<IClienteInserter> clienteInserters, ITcausrService tcausrService, IHttpContextAccessor httpContextAccessor, BaseDbContext dbContext)
        {
            _configuration = configuration;
            _clienteInserters = clienteInserters;
            _tcausrService = tcausrService;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
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
                    query.AppendLine("SELECT cve, nom, rfc, seg_mer, tel2,  Mail1, suc FROM cxccli");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new cxccli()
                            {
                                cve = dr["cve"].ToString(),
                                nom = dr["nom"].ToString(),
                                rfc = dr["rfc"].ToString(),
                                seg_mer = dr["seg_mer"].ToString(),
                                tel2 = dr["tel2"].ToString(),
                                Mail1 = dr["Mail1"].ToString(),
                                suc = dr["suc"].ToString(),
                                
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
        //LISTADO DE CLIENTES Y CONEXION SQLSERVER VISTA VCLIENTES
        public D_Cxccli(BaseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // LOGICA DE INSERCIÓN DE CLIENTES
        public void InsertarClienteCompleto(cxccli cliente, string ciaVentas, ITcausrService tcausrService)
        {
            var (nombreUsuario, tcausr) = ObtenerUsuarioActual(tcausrService);


            foreach (var inserter in _clienteInserters)
            {
                inserter.InsertarCliente(cliente, ciaVentas, _dbContext, nombreUsuario);
            }
        }

        //AUDITORIA DE OBTENCION DE NOMBRE DE USUARIO PARA CREACION Y EDICION
        private (string nombreUsuario, tcausr usuario) ObtenerUsuarioActual(ITcausrService tcausrService)
        {
            // Lógica para obtener el nombre del usuario actual desde HttpContext
            var nombreUsuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            // Intentar obtener el nombre de usuario de tu servicio ITcausrService
            var tcausr = _dbContext.Tcausr.FirstOrDefault(u => u.nom_cto == nombreUsuario);

            return (nombreUsuario, tcausr);
        }

        //AUDITORIA DE OBTENCION DE NOMBRE DE USUARIO PARA CREACION Y EDICION


        //INSERCION CLIENTE BASE CXCCLI

        //REGLA DEL SERVIDOR POR LA LONGITUD DEL CAMPO
        public void ValidarYAgregarCampo(SqlParameterCollection parametros, string nombreParametro, string valor, int longitudMaxima)
        {
            if (valor.Length > longitudMaxima)
            {
                throw new Exception($"La longitud del campo '{nombreParametro}' no puede exceder los {longitudMaxima} caracteres.");
            }
            else
            {
                parametros.Add($"@{nombreParametro}", SqlDbType.VarChar).Value = valor;
            }
        }
        //REGLA DEL SERVIDOR POR LA LONGITUD DEL CAMPO

        public void InsertarClienteBase(cxccli nuevoCliente, string ciaVentas, BaseDbContext dbContext, ITcausrService tcausrService)
        {   
            // Obtener información del usuario
            var (nombreUsuario, tcausr) = ObtenerUsuarioActual(tcausrService);

            using (SqlConnection oconexion = new SqlConnection(ciaVentas))
            {
                oconexion.Open();
                using (SqlTransaction transaction = oconexion.BeginTransaction())

                {
                    
                    try
                    {
                        // Iniciar transacción

                        // Obtener el último valor de cve insertado
                        string obtenerUltimoCveQuery = "SELECT TOP 1 cve FROM cxccli ORDER BY cve DESC";
                        SqlCommand obtenerUltimoCveCmd = new SqlCommand(obtenerUltimoCveQuery, oconexion, transaction);
                        string ultimoCve = obtenerUltimoCveCmd.ExecuteScalar() as string;

                        // Generar el nuevo valor cve
                        string nuevoCve = GenerarNuevoCve(ultimoCve);

                        nuevoCliente.usr_alta = tcausr?.nom_cto;
                        nuevoCliente.mod_usr = tcausr?.nom_cto;

                        // Paso 1: Insertar en la tabla principal (cxccli)
                        string insertarClienteQuery1 = @"
                    INSERT INTO cxccli (
                        ibuff, cia, cve, zon, Cobrador, rep, Vendedor, nom, dir1, dir2, cd, est,
                        pais, pos, ent_dir1, ent_dir2, ent_cd, ent_est, ent_pais, ent_pos, tel1, tel2, telex, fax, dia_rev0, dia_rev1, hr_rev0,
                        hr_rev1, dia_pag0, dia_pag1, hr_pag0, hr_pag1, moneda, con_nom0, con_nom1, con_nom2, con_pto0, con_pto1, con_pto2, con_tel0,
                        con_tel1, con_tel2, seg_mer, con_pag, cta_con, tpo_cli, for_pgo, env_nom, tpo_des, buf, tipo_dscto, mod_vta, conden, sal_ven,
                        mod_fec, mod_usr, fec_ult_vta, fec_ult_pag, precio, imp, plazo_pago, dias_pto_pago, num_pag, porc_dscto, des_fac, saldo,
                        lim_cre, moratorios, desc_pto_pago, t_cargos, t_abonos, emb_ruta, flag_msg, dia_ent1, dia_ent2, dia_ent3, dia_ent4, dia_ent5,
                        dia_ent6, dia_ent7, orden_ent, tipo, suc, fec_alta, hra_alta, usr_alta, apartado_p, status, env_pub, nom_com, dir3, fec_lim_cre,
                        usr_lim_cre, imp_aut_bonif, rfc, CURP, DiasProntoPago2, DiasProntoPago3, DiasProntoPago4, DescuentoPPago2, DescuentoPPago3,
                        DescuentoPPago4, Agrupacion1, Agrupacion2, Agrupacion3, PorcientoCargoReparto, RegistroIEPS, Fax2, Mail1, Mail2, Mail3,
                        DirInternet, Modulo, ApellidoPaterno, ApellidoMaterno, Nombre, ZonaCobro, RegistroAlcohol, num_int, id1, id2, id3
                    )
                    VALUES (
                        @ibuff, @cia, @cve, @zon, @Cobrador, @rep, @Vendedor, @nom, @dir1, @dir2, @cd, @est,
                        @pais, @pos, @ent_dir1, @ent_dir2, @ent_cd, @ent_est, @ent_pais, @ent_pos, @tel1, @tel2, @telex, @fax, @dia_rev0, @dia_rev1, @hr_rev0,
                        @hr_rev1, @dia_pag0, @dia_pag1, @hr_pag0, @hr_pag1, @moneda, @con_nom0, @con_nom1, @con_nom2, @con_pto0, @con_pto1, @con_pto2, @con_tel0,
                        @con_tel1, @con_tel2, @seg_mer, @con_pag, @cta_con, @tpo_cli, @for_pgo, @env_nom, @tpo_des, @buf, @tipo_dscto, @mod_vta, @conden, @sal_ven,
                        @mod_fec, @mod_usr, @fec_ult_vta, @fec_ult_pag, @precio, @imp, @plazo_pago, @dias_pto_pago, @num_pag, @porc_dscto, @des_fac, @saldo,
                        @lim_cre, @moratorios, @desc_pto_pago, @t_cargos, @t_abonos, @emb_ruta, @flag_msg, @dia_ent1, @dia_ent2, @dia_ent3, @dia_ent4, @dia_ent5,
                        @dia_ent6, @dia_ent7, @orden_ent, @tipo, @suc, @fec_alta, @hra_alta, @usr_alta, @apartado_p, @status, @env_pub, @nom_com, @dir3, @fec_lim_cre,
                        @usr_lim_cre, @imp_aut_bonif, @rfc, @CURP, @DiasProntoPago2, @DiasProntoPago3, @DiasProntoPago4, @DescuentoPPago2, @DescuentoPPago3,
                        @DescuentoPPago4, @Agrupacion1, @Agrupacion2, @Agrupacion3, @PorcientoCargoReparto, @RegistroIEPS, @Fax2, @Mail1, @Mail2, @Mail3,
                        @DirInternet, @Modulo, @ApellidoPaterno, @ApellidoMaterno, @Nombre, @ZonaCobro, @RegistroAlcohol, @num_int, @id1, @id2, @id3
                    )";

                        using (SqlCommand insertarClienteCmd1 = new SqlCommand(insertarClienteQuery1, oconexion, transaction))
                        {

                            insertarClienteCmd1.Parameters.Add("@ibuff", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@cia", SqlDbType.VarChar).Value = "MAS"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@cve", SqlDbType.VarChar).Value = nuevoCliente.cve; //PENDIENTE
                            ValidarYAgregarCampo(insertarClienteCmd1.Parameters, "zon", nuevoCliente.zon, 3); //SE SELECCIONA CATALOGO
                            insertarClienteCmd1.Parameters.Add("@Cobrador", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@rep", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            ValidarYAgregarCampo(insertarClienteCmd1.Parameters, "Vendedor", nuevoCliente.Vendedor, 9); //SE SELECCIONA CATALOGO
                            ValidarYAgregarCampo(insertarClienteCmd1.Parameters, "nom", nuevoCliente.nom, 50); //SE CAPTURA DESDE FORMULARIO
                            ValidarYAgregarCampo(insertarClienteCmd1.Parameters, "dir1", nuevoCliente.dir1, 30); //SE CAPTURA DESDE FORMULARIO
                            insertarClienteCmd1.Parameters.Add("@dir2", SqlDbType.VarChar).Value = nuevoCliente.dir1; //SE DUPLICA EL REGISTRO DEL CAMPO dir1
                            ValidarYAgregarCampo(insertarClienteCmd1.Parameters, "cd", nuevoCliente.cd, 15); //SE SELECCIONA CATALOGO comestados
                            ValidarYAgregarCampo(insertarClienteCmd1.Parameters, "est", nuevoCliente.est, 3); //AL SELECCIOMAR REGISTRODE DEL CATALOGO comestados SE INSERTA DE FORMA AUTOMATICA
                            insertarClienteCmd1.Parameters.Add("@pais", SqlDbType.VarChar).Value = "MEX"; //SE INSERTA DIRECTO
                            ValidarYAgregarCampo(insertarClienteCmd1.Parameters, "pos", nuevoCliente.pos, 6); //SE SELECCIONA CATALOGO ccecpo
                            insertarClienteCmd1.Parameters.Add("@ent_dir1", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@ent_dir2", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@ent_cd", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@ent_est", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@ent_pais", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@ent_pos", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@tel1", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            ValidarYAgregarCampo(insertarClienteCmd1.Parameters, "tel2", nuevoCliente.tel2, 13); //SE CAPTURA DESDE FORMULARIO
                            insertarClienteCmd1.Parameters.Add("@telex", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@fax", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@dia_rev0", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@dia_rev1", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@hr_rev0", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@hr_rev1", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@dia_pag0", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@dia_pag1", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@hr_pag0", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@hr_pag1", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@moneda", SqlDbType.VarChar).Value = "NAL"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@con_nom0", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@con_nom1", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@con_nom2", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            ValidarYAgregarCampo(insertarClienteCmd1.Parameters, "con_pto0", nuevoCliente.con_pto0, 20); //SE CAPTURA DESDE FORMULARIO GIRO COMERCIAL
                            insertarClienteCmd1.Parameters.Add("@con_pto1", SqlDbType.VarChar).Value = "INTRANET"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@con_pto2", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@con_tel0", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@con_tel1", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@con_tel2", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            ValidarYAgregarCampo(insertarClienteCmd1.Parameters, "seg_mer", nuevoCliente.seg_mer, 3); //SE SELECCIONA CATALOGO segmer
                            ValidarYAgregarCampo(insertarClienteCmd1.Parameters, "con_pag", nuevoCliente.con_pag, 5); //SE SELECCIONA CATALOGO cxccon
                            insertarClienteCmd1.Parameters.Add("@cta_con", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@tpo_cli", SqlDbType.VarChar).Value = "1"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@for_pgo", SqlDbType.VarChar).Value = "EFEVO"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@env_nom", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@tpo_des", SqlDbType.VarChar).Value = "1"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@buf", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@tipo_dscto", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@mod_vta", SqlDbType.VarChar).Value = "N"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@conden", SqlDbType.VarChar).Value = "N"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@sal_ven", SqlDbType.VarChar).Value = "N"; //SE INSERTA DIRECTO
                            ValidarYAgregarCampo(insertarClienteCmd1.Parameters, "mod_fec", nuevoCliente.mod_fec, 8); //SE CAPTURA DESDE FORMULARIO
                            insertarClienteCmd1.Parameters.Add("@mod_usr", SqlDbType.VarChar).Value = tcausr.nom_cto; //De la interfaz ITcausrService
                            insertarClienteCmd1.Parameters.Add("@fec_ult_vta", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@fec_ult_pag", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@precio", SqlDbType.VarChar).Value = "0"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@imp", SqlDbType.VarChar).Value = "1"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@plazo_pago", SqlDbType.VarChar).Value = "0"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@dias_pto_pago", SqlDbType.VarChar).Value = "0"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@num_pag", SqlDbType.VarChar).Value = "0"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@porc_dscto", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@des_fac", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@saldo", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@lim_cre", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@moratorios", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@desc_pto_pago", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@t_cargos", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@t_abonos", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@emb_ruta", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@flag_msg", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@dia_ent1", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@dia_ent2", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@dia_ent3", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@dia_ent4", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@dia_ent5", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@dia_ent6", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@dia_ent7", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@orden_ent", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@tipo", SqlDbType.VarChar).Value = "N"; //SE INSERTA DIRECTO
                            ValidarYAgregarCampo(insertarClienteCmd1.Parameters, "suc", nuevoCliente.pos, 3); //SE SELECCIONA CATALOGO comsuc
                            ValidarYAgregarCampo(insertarClienteCmd1.Parameters, "fec_alta", nuevoCliente.fec_alta, 8); //SE CAPTURA DESDE FORMULARIO
                            ValidarYAgregarCampo(insertarClienteCmd1.Parameters, "hra_alta", nuevoCliente.hra_alta, 6); //SE CAPTURA DESDE FORMULARIO
                            insertarClienteCmd1.Parameters.Add("@usr_alta", SqlDbType.VarChar).Value = tcausr.nom_cto; //De la interfaz ITcausrService
                            insertarClienteCmd1.Parameters.Add("@apartado_p", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@status", SqlDbType.VarChar).Value = "2"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@env_pub", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@nom_com", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@dir3", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@fec_lim_cre", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@usr_lim_cre", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@imp_aut_bonif", SqlDbType.VarChar).Value = "N"; //SE INSERTA DIRECTO
                            ValidarYAgregarCampo(insertarClienteCmd1.Parameters, "rfc", nuevoCliente.rfc, 20); //SE CAPTURA DESDE FORMULARIO
                            insertarClienteCmd1.Parameters.Add("@CURP", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@DiasProntoPago2", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@DiasProntoPago3", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@DiasProntoPago4", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@DescuentoPPago2", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@DescuentoPPago3", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@DescuentoPPago4", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@Agrupacion1", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@Agrupacion2", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@Agrupacion3", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@PorcientoCargoReparto", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@RegistroIEPS", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@Fax2", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            ValidarYAgregarCampo(insertarClienteCmd1.Parameters, "Mail1", nuevoCliente.Mail1, 60); //SE CAPTURA DESDE FORMULARIO
                            insertarClienteCmd1.Parameters.Add("@Mail2", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@Mail3", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@DirInternet", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@Modulo", SqlDbType.VarChar).Value = "C"; //SE INSERTA DIRECTO
                            insertarClienteCmd1.Parameters.Add("@ApellidoPaterno", SqlDbType.VarChar).Value = nuevoCliente.nom; //SE DUPLICA EL REGISTRO DEL CAMPO nom
                            insertarClienteCmd1.Parameters.Add("@ApellidoMaterno", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@ZonaCobro", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@RegistroAlcohol", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@num_int", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@id1", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@id2", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd1.Parameters.Add("@id3", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO

                            insertarClienteCmd1.ExecuteNonQuery();
                        }

                        // Commit si todo fue exitoso
                        transaction.Commit();
                    }
                    catch (SqlException sqlEx)
                    {
                        // Manejar excepciones de SQL
                        Console.WriteLine("Error SQL: " + sqlEx.Message);
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        Console.WriteLine("Ocurrió un error al procesar la solicitud. Por favor, inténtelo nuevamente.");
                        return;
                    }

                }
            }
        }
        //INSERCION CLIENTE BASE CXCCLI


        //INSERCION HERENCIA CXCCLI_AD
        private void InsertarCliente(cxccli_ad clienteAd, string ciaVentas, BaseDbContext dbContext, ITcausrService tcausrService)
        {
            // Obtener información del usuario
            var (nombreUsuario, tcausr) = ObtenerUsuarioActual(tcausrService);

            using (SqlConnection oconexion = new SqlConnection(ciaVentas))
            {
                oconexion.Open();
                using (SqlTransaction transaction = oconexion.BeginTransaction())

                {
                    try
                    {
                        // Iniciar transacción

                        clienteAd.u_mod = tcausr?.nom_cto;

                        // Paso 2: Insertar en la segunda tabla (cxclic_ad)
                        string insertarClienteQuery2 = @"
                    INSERT INTO cxccli_ad (
                        cia, cve, campo, valor_str1, valor_int, valor_float, f_mod, h_mod, u_mod
                    )
                    VALUES (
                        @ciaEnCxccli, @cveEnCxccli, @campo, @valor_str1EnCxccli, @valor_str1, @valor_int, @valor_float, @f_mod, @h_mod, @u_mod
                    )";
                        using (SqlCommand insertarClienteCmd2 = new SqlCommand(insertarClienteQuery2, oconexion, transaction))
                        {

                            insertarClienteCmd2.Parameters.Add("@ciaEnCxccli", SqlDbType.VarChar).Value = clienteAd.ciaEnCxccli; //SE INSERTA DIRECTO
                            insertarClienteCmd2.Parameters.Add("@cveEnCxccli", SqlDbType.VarChar).Value = clienteAd.cveEnCxccli; //PENDIENTE
                            insertarClienteCmd2.Parameters.Add("@campo", SqlDbType.VarChar).Value = "NombreCliente"; //SE INSERTA DIRECTO
                            insertarClienteCmd2.Parameters.Add("@valor_str1", SqlDbType.VarChar).Value = clienteAd.valor_str1EnCxccli; //PENDIENTE
                            insertarClienteCmd2.Parameters.Add("@valor_int", SqlDbType.VarChar).Value = "0"; //SE INSERTA DIRECTO
                            insertarClienteCmd2.Parameters.Add("@valor_float", SqlDbType.VarChar).Value = "0"; //SE INSERTA DIRECTO
                            ValidarYAgregarCampo(insertarClienteCmd2.Parameters, "f_mod", clienteAd.f_mod, 8); //SE CAPTURA DESDE FORMULARIO
                            ValidarYAgregarCampo(insertarClienteCmd2.Parameters, "h_mod", clienteAd.h_mod, 6); //SE CAPTURA DESDE FORMULARIO
                            insertarClienteCmd2.Parameters.Add("@u_mod", SqlDbType.VarChar).Value = tcausr.nom_cto; //De la interfaz ITcausrService
                            //SE REPITE LOGICA UNA VEZ MAS PARA CADA NUEVO REGISTRO DE CLIENTE
                            insertarClienteCmd2.Parameters.Add("@cia", SqlDbType.VarChar).Value = "MAS"; //SE INSERTA DIRECTO
                            insertarClienteCmd2.Parameters.Add("@cve", SqlDbType.VarChar).Value = clienteAd.cveEnCxccli; //PENDIENTE
                            insertarClienteCmd2.Parameters.Add("@campo", SqlDbType.VarChar).Value = "RegimenFiscal"; //SE INSERTA DIRECTO
                            ValidarYAgregarCampo(insertarClienteCmd2.Parameters, "@valor_str1", clienteAd.valor_str1, 300); //SE SELECCIONA CATALOGO cceuso_ad
                            insertarClienteCmd2.Parameters.Add("@valor_int", SqlDbType.VarChar).Value = "0"; //SE INSERTA DIRECTO
                            insertarClienteCmd2.Parameters.Add("@valor_float", SqlDbType.VarChar).Value = "0"; //SE INSERTA DIRECTO
                            ValidarYAgregarCampo(insertarClienteCmd2.Parameters, "@f_mod", clienteAd.f_mod, 8); //SE CAPTURA DESDE FORMULARIO
                            ValidarYAgregarCampo(insertarClienteCmd2.Parameters, "@h_mod", clienteAd.h_mod, 6); //SE CAPTURA DESDE FORMULARIO
                            insertarClienteCmd2.Parameters.Add("@u_mod", SqlDbType.VarChar).Value = tcausr.nom_cto; //De la interfaz ITcausrService

                            insertarClienteCmd2.ExecuteNonQuery();
                        }

                        // Commit si todo fue exitoso
                        transaction.Commit();
                    }
                    catch (SqlException sqlEx)
                    {
                        // Manejar excepciones de SQL
                        Console.WriteLine("Error SQL: " + sqlEx.Message);
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        Console.WriteLine("Ocurrió un error al procesar la solicitud. Por favor, inténtelo nuevamente.");
                        return;
                    }
                }
            }
        }
        //INSERCION HERENCIA CXCCLI_AD


        //INSERCION HERENCIA CXCCLI_SAT
        private void InsertarCliente(cxccli_sat clienteSat, string ciaVentas, BaseDbContext dbContext, ITcausrService tcausrService)
        {
            // Obtener información del usuario
            var (nombreUsuario, tcausr) = ObtenerUsuarioActual(tcausrService);

            using (SqlConnection oconexion = new SqlConnection(ciaVentas))
            {
                oconexion.Open();
                using (SqlTransaction transaction = oconexion.BeginTransaction())

                {
                    try
                    {
                        // Iniciar transacción

                        clienteSat.u_alta = tcausr?.nom_cto;
                        clienteSat.u_mod = tcausr?.nom_cto;

                        // Paso 3: Insertar en la tercera tabla (cxclic_sat)
                        string insertarClienteQuery3 = @"
                    INSERT INTO cxclic_sat (
                        cia, cve, usocfdi, f_alta, h_alta, u_alta, f_mod, h_mod, u_mod
                    )
                    VALUES (
                        @ciaEnCxccli, @cveEnCxccli, @usocfdi, @f_alta, @h_alta, @u_alta, @f_mod, @h_mod, @u_mod
                    )";
                        using (SqlCommand insertarClienteCmd3 = new SqlCommand(insertarClienteQuery3, oconexion, transaction))
                        {
                            insertarClienteCmd3.Parameters.Add("@ciaEnCxccli", SqlDbType.VarChar).Value = clienteSat.ciaEnCxccli; //SE INSERTA DIRECTO
                            insertarClienteCmd3.Parameters.Add("@cveEnCxccli", SqlDbType.VarChar).Value = clienteSat.cveEnCxccli; //PENDIENTE
                            ValidarYAgregarCampo(insertarClienteCmd3.Parameters, "@usocfdi", clienteSat.usocfdi, 3); //SE SELECCIONA CATALOGO cceuso
                            ValidarYAgregarCampo(insertarClienteCmd3.Parameters, "@f_alta", clienteSat.f_alta, 8); //SE CAPTURA DESDE FORMULARIO
                            ValidarYAgregarCampo(insertarClienteCmd3.Parameters, "@h_alta", clienteSat.h_alta, 6); //SE CAPTURA DESDE FORMULARIO
                            insertarClienteCmd3.Parameters.Add("@u_alta", SqlDbType.VarChar).Value = tcausr.nom_cto; //De la interfaz ITcausrService
                            ValidarYAgregarCampo(insertarClienteCmd3.Parameters, "@f_mod", clienteSat.f_mod, 8); //SE CAPTURA DESDE FORMULARIO
                            ValidarYAgregarCampo(insertarClienteCmd3.Parameters, "@h_mod", clienteSat.h_mod, 6); //SE CAPTURA DESDE FORMULARIO
                            insertarClienteCmd3.Parameters.Add("@u_mod", SqlDbType.VarChar).Value = tcausr.nom_cto; //De la interfaz ITcausrService

                            insertarClienteCmd3.ExecuteNonQuery();
                        }
                        // Commit si todo fue exitoso
                        transaction.Commit();
                    }
                    catch (SqlException sqlEx)
                    {
                        // Manejar excepciones de SQL
                        Console.WriteLine("Error SQL: " + sqlEx.Message);
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        Console.WriteLine("Ocurrió un error al procesar la solicitud. Por favor, inténtelo nuevamente.");
                        return;
                    }
                }
            }
        }
        //INSERCION HERENCIA CXCCLI_SAT


        //INSERCION HERENCIA CXCDIR
        private void InsertarCliente(cxcdir clienteDir, string ciaVentas, BaseDbContext dbContext, ITcausrService tcausrService)
        {
            // Obtener información del usuario
            var (nombreUsuario, tcausr) = ObtenerUsuarioActual(tcausrService);

            using (SqlConnection oconexion = new SqlConnection(ciaVentas))
            {
                oconexion.Open();
                using (SqlTransaction transaction = oconexion.BeginTransaction())

                {
                    try
                    {
                        // Iniciar transacción

                        clienteDir.u_mod = tcausr?.nom_cto;

                        // Paso 4: Insertar en la cuarta tabla (cxclic_sat)
                        string insertarClienteQuery4 = @"
                    INSERT INTO cxcdir (
                        ibuff, cia, cve_cli, num, env_nom, dir1, dir2, cd, est, pais, pos, tel1, tel2,
                        fax, rep, emb_ruta, orden_ent, f_mod, h_mod, u_mod, Latitud, Longitud, Km, IncluyeCartaPorte
                    )
                    VALUES (
                        @ibuffEnCxccli, @ciaEnCxccli, @cve_cliEnCxccli, @num, @env_nom, @dir1EnCxccli, @dir2EnCxccli, @cdEnCxccli, @estEnCxccli, @paisEnCxccli, @posEnCxccli, @tel1EnCxccli, @tel2EnCxccli,
                        @faxEnCxccli, @rep, @emb_ruta, @orden_ent, @f_mod, @h_mod, @u_mod, @Latitud, @Longitud, @Km, @IncluyeCartaPorte
                    )";
                        using (SqlCommand insertarClienteCmd4 = new SqlCommand(insertarClienteQuery4, oconexion, transaction))
                        {
                            insertarClienteCmd4.Parameters.Add("@ibuffEnCxccli", SqlDbType.VarChar).Value = clienteDir.ibuffEnCxccli; //SE INSERTA DIRECTO
                            insertarClienteCmd4.Parameters.Add("@ciaEnCxccli", SqlDbType.VarChar).Value = clienteDir.ciaEnCxccli; //SE INSERTA DIRECTO
                            insertarClienteCmd4.Parameters.Add("@cve_cliEnCxccli", SqlDbType.VarChar).Value = clienteDir.cve_cliEnCxccli; //PENDIENTE
                            insertarClienteCmd4.Parameters.Add("@num", SqlDbType.VarChar).Value = "000"; //SE INSERTA DIRECTO
                            insertarClienteCmd4.Parameters.Add("@env_nom", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd4.Parameters.Add("@dir1EnCxccli", SqlDbType.VarChar).Value = clienteDir.dir1EnCxccli; //SE INSERTA DIRECTO
                            insertarClienteCmd4.Parameters.Add("@dir2EnCxccli", SqlDbType.VarChar).Value = clienteDir.dir2EnCxccli; //SE INSERTA DIRECTO
                            insertarClienteCmd4.Parameters.Add("@cdEnCxccli", SqlDbType.VarChar).Value = clienteDir.cdEnCxccli; //SE INSERTA DIRECTO
                            insertarClienteCmd4.Parameters.Add("@estEnCxccli", SqlDbType.VarChar).Value = clienteDir.estEnCxccli; //SE INSERTA DIRECTO
                            insertarClienteCmd4.Parameters.Add("@paisEnCxccli", SqlDbType.VarChar).Value = clienteDir.paisEnCxccli; //SE INSERTA DIRECTO
                            insertarClienteCmd4.Parameters.Add("@posEnCxccli", SqlDbType.VarChar).Value = clienteDir.posEnCxccli; //SE INSERTA DIRECTO
                            insertarClienteCmd4.Parameters.Add("@tel1EnCxccli", SqlDbType.VarChar).Value = clienteDir.tel1EnCxccli; //SE INSERTA DIRECTO
                            insertarClienteCmd4.Parameters.Add("@tel2EnCxccli", SqlDbType.VarChar).Value = clienteDir.tel2EnCxccli; //SE INSERTA DIRECTO
                            insertarClienteCmd4.Parameters.Add("@faxEnCxccli", SqlDbType.VarChar).Value = clienteDir.faxEnCxccli; //SE INSERTA DIRECTO
                            insertarClienteCmd4.Parameters.Add("@rep", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd4.Parameters.Add("@emb_ruta", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            insertarClienteCmd4.Parameters.Add("@orden_ent", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            ValidarYAgregarCampo(insertarClienteCmd4.Parameters, "@f_mod", clienteDir.f_mod, 8); //SE CAPTURA DESDE FORMULARIO
                            ValidarYAgregarCampo(insertarClienteCmd4.Parameters, "@h_mod", clienteDir.h_mod, 6); //SE CAPTURA DESDE FORMULARIO
                            insertarClienteCmd4.Parameters.Add("@u_mod", SqlDbType.VarChar).Value = tcausr.nom_cto; //De la interfaz ITcausrService
                            insertarClienteCmd4.Parameters.Add("@Latitud", SqlDbType.VarChar).Value = "0"; //SE INSERTA DIRECTO
                            insertarClienteCmd4.Parameters.Add("@Longitud", SqlDbType.VarChar).Value = "0"; //SE INSERTA DIRECTO
                            insertarClienteCmd4.Parameters.Add("@Km", SqlDbType.VarChar).Value = "0"; //SE INSERTA DIRECTO
                            insertarClienteCmd4.Parameters.Add("@IncluyeCartaPorte", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO

                            insertarClienteCmd4.ExecuteNonQuery();
                        }
                        // Commit si todo fue exitoso
                        transaction.Commit();
                    }
                    catch (SqlException sqlEx)
                    {
                        // Manejar excepciones de SQL
                        Console.WriteLine("Error SQL: " + sqlEx.Message);
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        Console.WriteLine("Ocurrió un error al procesar la solicitud. Por favor, inténtelo nuevamente.");
                        return;
                    }
                }
            }
        }
        //INSERCION HERENCIA CXCDIR


        //INSERCION HERENCIA CXCFPG
        private void InsertarCliente(cxcfpg clienteFpg, string ciaVentas, BaseDbContext dbContext, ITcausrService tcausrService)
        {
            // Obtener información del usuario
            var (nombreUsuario, tcausr) = ObtenerUsuarioActual(tcausrService);

            using (SqlConnection oconexion = new SqlConnection(ciaVentas))
            {
                oconexion.Open();
                using (SqlTransaction transaction = oconexion.BeginTransaction())

                {
                    try
                    {
                        // Iniciar transacción

                        // Paso 5: Insertar en la tercera tabla (cxcfpg)
                        string insertarClienteQuery5 = @"
                    INSERT INTO cxcfpg (
                        Cia, Cliente, Codigo, Par, Condicion
                    )
                    VALUES (
                        @CiaEnCxccli, @ClienteEnCxccli, @Codigo, @Par, @Condicion
                    )";
                        using (SqlCommand insertarClienteCmd5 = new SqlCommand(insertarClienteQuery5, oconexion, transaction))
                        {
                            //SE REPITE 5 VECES
                            //1
                            insertarClienteCmd5.Parameters.Add("@CiaEnCxccli", SqlDbType.VarChar).Value = clienteFpg.CiaEnCxccli; //SE INSERTA DIRECTO
                            insertarClienteCmd5.Parameters.Add("@ClienteEnCxccli", SqlDbType.VarChar).Value = clienteFpg.ClienteEnCxccli; //PENDIENTE
                            insertarClienteCmd5.Parameters.Add("@Codigo", SqlDbType.VarChar).Value = "CHEQ"; //SE INSERTA DIRECTO
                            insertarClienteCmd5.Parameters.Add("@Par", SqlDbType.VarChar).Value = "1"; //SE INSERTA DIRECTO
                            insertarClienteCmd5.Parameters.Add("@Condicion", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            //2
                            insertarClienteCmd5.Parameters.Add("@CiaEnCxccli", SqlDbType.VarChar).Value = clienteFpg.CiaEnCxccli; //SE INSERTA DIRECTO
                            insertarClienteCmd5.Parameters.Add("@ClienteEnCxccli", SqlDbType.VarChar).Value = clienteFpg.ClienteEnCxccli; //PENDIENTE
                            insertarClienteCmd5.Parameters.Add("@Codigo", SqlDbType.VarChar).Value = "EFEVO"; //SE INSERTA DIRECTO
                            insertarClienteCmd5.Parameters.Add("@Par", SqlDbType.VarChar).Value = "2"; //SE INSERTA DIRECTO
                            insertarClienteCmd5.Parameters.Add("@Condicion", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            //3
                            insertarClienteCmd5.Parameters.Add("@CiaEnCxccli", SqlDbType.VarChar).Value = clienteFpg.CiaEnCxccli; //SE INSERTA DIRECTO
                            insertarClienteCmd5.Parameters.Add("@ClienteEnCxccli", SqlDbType.VarChar).Value = clienteFpg.ClienteEnCxccli; //PENDIENTE
                            insertarClienteCmd5.Parameters.Add("@Codigo", SqlDbType.VarChar).Value = "SANCRE"; //SE INSERTA DIRECTO
                            insertarClienteCmd5.Parameters.Add("@Par", SqlDbType.VarChar).Value = "3"; //SE INSERTA DIRECTO
                            insertarClienteCmd5.Parameters.Add("@Condicion", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            //4
                            insertarClienteCmd5.Parameters.Add("@CiaEnCxccli", SqlDbType.VarChar).Value = clienteFpg.CiaEnCxccli; //SE INSERTA DIRECTO
                            insertarClienteCmd5.Parameters.Add("@ClienteEnCxccli", SqlDbType.VarChar).Value = clienteFpg.ClienteEnCxccli; //PENDIENTE
                            insertarClienteCmd5.Parameters.Add("@Codigo", SqlDbType.VarChar).Value = "SANDEB"; //SE INSERTA DIRECTO
                            insertarClienteCmd5.Parameters.Add("@Par", SqlDbType.VarChar).Value = "4"; //SE INSERTA DIRECTO
                            insertarClienteCmd5.Parameters.Add("@Condicion", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO
                            //5
                            insertarClienteCmd5.Parameters.Add("@CiaEnCxccli", SqlDbType.VarChar).Value = clienteFpg.CiaEnCxccli; //SE INSERTA DIRECTO
                            insertarClienteCmd5.Parameters.Add("@ClienteEnCxccli", SqlDbType.VarChar).Value = clienteFpg.ClienteEnCxccli; //PENDIENTE
                            insertarClienteCmd5.Parameters.Add("@Codigo", SqlDbType.VarChar).Value = "TRANSF"; //SE INSERTA DIRECTO
                            insertarClienteCmd5.Parameters.Add("@Par", SqlDbType.VarChar).Value = "5"; //SE INSERTA DIRECTO
                            insertarClienteCmd5.Parameters.Add("@Condicion", SqlDbType.VarChar).Value = ""; //QUEDA EN BLANCO

                            insertarClienteCmd5.ExecuteNonQuery();
                }
                // Commit si todo fue exitoso
                transaction.Commit();
            }
            catch (SqlException sqlEx)
            {
                // Manejar excepciones de SQL
                Console.WriteLine("Error SQL: " + sqlEx.Message);
                        return;
                    }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("Ocurrió un error al procesar la solicitud. Por favor, inténtelo nuevamente.");
                        return;
                    }
                }
            }
        }
        //INSERCION HERENCIA CXCCFG


        //INSERCION HERENCIA CFRCLI
        private void InsertarCliente(cfrcli clienteCfr, string ciaVentas, BaseDbContext dbContext, ITcausrService tcausrService)
        {
            // Obtener información del usuario
            var (nombreUsuario, tcausr) = ObtenerUsuarioActual(tcausrService);

            using (SqlConnection oconexion = new SqlConnection(ciaVentas))
            {
                oconexion.Open();
                using (SqlTransaction transaction = oconexion.BeginTransaction())
                {
                    try
                    {
                        // Iniciar transacción

                        clienteCfr.u_alta = tcausr?.nom_cto;
                        clienteCfr.u_mod = tcausr?.nom_cto;

                        // Paso 5: Insertar en la tercera tabla (cfrcli)
                        string insertarClienteQuery6 = @"
                    INSERT INTO cfrcli (
                        CodBarras, Cte, Nom, Dir1, Dir2, Cd, Edo, Pais, Cp, Tel1, Tel2, RFC, email, TotalPuntos,
                        SaldoPuntos, TotalMonedero, SaldoMonedero, TotalVenta, TotalDsc, TotalImpto, NumVtas,
                        f_alt, h_alt, u_alt, f_mod, h_mod, u_mod, Id_Tarjeta, SegMer, SaldoMonederoCpr, SaldoMonederoCer, SaldoMonederoOtro
                    )
                    VALUES (
                        @CodBarras, @CteEnCxccli, @NomEnCxccli, @Dir1EnCxccli, @Dir2EnCxccli, @CdEnCxccli, @EdoEnCxccli, @PaisEnCxccli,
                        @CpEnCxccli, @Tel1EnCxccli, @Tel2EnCxccli, @RFCEnCxccli, @emailEnCxccli, @TotalPuntos, @SaldoPuntos,
                        @TotalMonedero, @SaldoMonedero, @TotalVenta, @TotalDsc, @TotalImpto, @NumVtas, @f_alt, @h_alt,
                        @u_alt, @f_mod, @h_mod, @u_mod, @Id_Tarjeta, @SegMerEnCxccli, @SaldoMonederoCpr, @SaldoMonederoCer, @SaldoMonederoOtro
                    )";
                        using (SqlCommand insertarClienteCmd6 = new SqlCommand(insertarClienteQuery6, oconexion, transaction))
                        {
                            ValidarYAgregarCampo(insertarClienteCmd6.Parameters, "@CodBarras", clienteCfr.CodBarras, 13); //SE CAPTURA DESDE FORMULARIO
                            insertarClienteCmd6.Parameters.Add("@CteEnCxccli", SqlDbType.VarChar).Value = clienteCfr.CteEnCxccli; //PENDIENTE
                            insertarClienteCmd6.Parameters.Add("@NomEnCxccli", SqlDbType.VarChar).Value = clienteCfr.NomEnCxccli; //PENDIENTE
                            insertarClienteCmd6.Parameters.Add("@Dir1EnCxccli", SqlDbType.VarChar).Value = clienteCfr.Dir1EnCxccli; //PENDIENTE
                            insertarClienteCmd6.Parameters.Add("@Dir2EnCxccli", SqlDbType.VarChar).Value = clienteCfr.Dir2EnCxccli; //PENDIENTE
                            insertarClienteCmd6.Parameters.Add("@CdEnCxccli", SqlDbType.VarChar).Value = clienteCfr.CdEnCxccli; //PENDIENTE
                            insertarClienteCmd6.Parameters.Add("@EdoEnCxccli", SqlDbType.VarChar).Value = clienteCfr.EdoEnCxccli; //PENDIENTE
                            insertarClienteCmd6.Parameters.Add("@PaisEnCxccli", SqlDbType.VarChar).Value = clienteCfr.PaisEnCxccli; //PENDIENTE
                            insertarClienteCmd6.Parameters.Add("@CpEnCxccli", SqlDbType.VarChar).Value = clienteCfr.CpEnCxccli; //PENDIENTE
                            insertarClienteCmd6.Parameters.Add("@Tel1EnCxccli", SqlDbType.VarChar).Value = clienteCfr.Tel1EnCxccli; //PENDIENTE
                            insertarClienteCmd6.Parameters.Add("@Tel2EnCxccli", SqlDbType.VarChar).Value = clienteCfr.Tel2EnCxccli; //PENDIENTE
                            insertarClienteCmd6.Parameters.Add("@RFCEnCxccli", SqlDbType.VarChar).Value = clienteCfr.RFCEnCxccli; //PENDIENTE
                            insertarClienteCmd6.Parameters.Add("@emailEnCxccli", SqlDbType.VarChar).Value = clienteCfr.emailEnCxccli; //PENDIENTE
                            insertarClienteCmd6.Parameters.Add("@TotalPuntos", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd6.Parameters.Add("@SaldoPuntos", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd6.Parameters.Add("@TotalMonedero", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd6.Parameters.Add("@SaldoMonedero", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd6.Parameters.Add("@TotalVenta", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd6.Parameters.Add("@TotalDsc", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd6.Parameters.Add("@TotalImpto", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd6.Parameters.Add("@NumVtas", SqlDbType.VarChar).Value = "0"; //SE INSERTA DIRECTO
                            ValidarYAgregarCampo(insertarClienteCmd6.Parameters, "@f_alt", clienteCfr.f_alt, 8); //SE CAPTURA DESDE FORMULARIO
                            ValidarYAgregarCampo(insertarClienteCmd6.Parameters, "@h_alt", clienteCfr.h_alt, 6); //SE CAPTURA DESDE FORMULARIO
                            insertarClienteCmd6.Parameters.Add("@u_alta", SqlDbType.VarChar).Value = tcausr.nom_cto; //De la interfaz ITcausrService
                            ValidarYAgregarCampo(insertarClienteCmd6.Parameters, "@f_mod", clienteCfr.f_alt, 8); //SE CAPTURA DESDE FORMULARIO
                            ValidarYAgregarCampo(insertarClienteCmd6.Parameters, "@h_mod", clienteCfr.h_alt, 6); //SE CAPTURA DESDE FORMULARIO
                            insertarClienteCmd6.Parameters.Add("@u_mod", SqlDbType.VarChar).Value = tcausr.nom_cto; //De la interfaz ITcausrService
                            ValidarYAgregarCampo(insertarClienteCmd6.Parameters, "@Id_Tarjeta", clienteCfr.Id_Tarjeta, 30); //SE CAPTURA DESDE FORMULARIO
                            ValidarYAgregarCampo(insertarClienteCmd6.Parameters, "@SegMerEnCxccli", clienteCfr.SegMerEnCxccli, 30); //SE CAPTURA DESDE FORMULARIO
                            insertarClienteCmd6.Parameters.Add("@SaldoMonederoCpr", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd6.Parameters.Add("@SaldoMonederoCer", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO
                            insertarClienteCmd6.Parameters.Add("@SaldoMonederoOtro", SqlDbType.VarChar).Value = "0.00"; //SE INSERTA DIRECTO

                            insertarClienteCmd6.ExecuteNonQuery();
                        }
                        // Commit si todo fue exitoso
                        transaction.Commit();
                    }
                    catch (SqlException sqlEx)
                    {
                        // Manejar excepciones de SQL
                        Console.WriteLine("Error SQL: " + sqlEx.Message);
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        Console.WriteLine("Ocurrió un error al procesar la solicitud. Por favor, inténtelo nuevamente.");
                        return;
                    }
                }
            }
        }
            
        //INSERCION HERENCIA CFRCLI


        // GENERAR CLAVE CONSECUTIVA UNO A UNO
        private string GenerarNuevoCve(string ultimoCve)
        {
            int valorBase = 100000001; // Valor desde el cual comenzar

            if (string.IsNullOrEmpty(ultimoCve))
            {
                // Si no hay registros previos, comenzar desde el valor base
                return valorBase.ToString();
            }
            else
            {
                // Intentar convertir la parte numérica existente y asegurarse de que sea válida
                if (int.TryParse(ultimoCve, out int parteNumerica))
                {
                    // Incrementar la parte numérica
                    parteNumerica++;
                    return parteNumerica.ToString();
                }
                else
                {
                    // En caso de error, comenzar desde el valor base
                    return valorBase.ToString();
                }
            }
        }
        // GENERAR CLAVE CONSECUTIVA UNO A UNO
    }
}