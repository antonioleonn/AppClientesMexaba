using System;
using System.Collections.Generic;

namespace AppClientesMexaba.Models;

public class cxccli
{
    //NATIVAMENTE SON 122 CAMPOS
    public string ibuff { get; set; }
    public string cia { get; set; }
    public string cve { get; set; }
    public string zon { get; set; }
    public string Cobrador { get; set; }
    public string rep { get; set; }
    public string Vendedor { get; set; }
    public string nom { get; set; }
    public string dir1 { get; set; }
    public string dir2 { get; set; }
    public string cd { get; set; }
    public string est { get; set; }
    public string pais { get; set; }
    public string pos { get; set; }
    public string ent_dir1 { get; set; }
    public string ent_dir2 { get; set; }
    public string ent_cd { get; set; }
    public string ent_est { get; set; }
    public string ent_pais { get; set; }
    public string ent_pos { get; set; }
    public string tel1 { get; set; }
    public string tel2 { get; set; }
    public string telex { get; set; }
    public string fax { get; set; }
    public string dia_rev0 { get; set; }
    public string dia_rev1 { get; set; }
    public string hr_rev0 { get; set; }
    public string hr_rev1 { get; set; }
    public string dia_pag0 { get; set; }
    public string dia_pag1 { get; set; }
    public string hr_pag0 { get; set; }
    public string hr_pag1 { get; set; }
    public string moneda { get; set; }
    public string con_nom0 { get; set; }
    public string con_nom1 { get; set; }
    public string con_nom2 { get; set; }
    public string con_pto0 { get; set; }
    public string con_pto1 { get; set; }
    public string con_pto2 { get; set; }
    public string con_tel0 { get; set; }
    public string con_tel1 { get; set; }
    public string con_tel2 { get; set; }
    public string seg_mer { get; set; }
    public string con_pag { get; set; }
    public string cta_con { get; set; }
    public string tpo_cli { get; set; }
    public string for_pgo { get; set; }
    public string env_nom { get; set; }
    public string tpo_des { get; set; }
    public string buf { get; set; }
    public string tipo_dscto { get; set; }
    public string mod_vta { get; set; }
    public string conden { get; set; }
    public string sal_ven { get; set; }
    public string mod_fec { get; set; }
    public string mod_usr { get; set; } //De la interfaz ITcausrService
    public string fec_ult_vta { get; set; }
    public string fec_ult_pag { get; set; }
    public short precio { get; set; }
    public short imp { get; set; }
    public short plazo_pago { get; set; }
    public short dias_pto_pago { get; set; }
    public short num_pag { get; set; }
    public decimal porc_dscto { get; set; }
    public decimal des_fac { get; set; }
    public decimal saldo { get; set; }
    public decimal lim_cre { get; set; }
    public decimal moratorios { get; set; }
    public decimal desc_pto_pago { get; set; }
    public decimal t_cargos { get; set; }
    public decimal t_abonos { get; set; }
    public string emb_ruta { get; set; }
    public string flag_msg { get; set; }
    public string dia_ent1 { get; set; }
    public string dia_ent2 { get; set; }
    public string dia_ent3 { get; set; }
    public string dia_ent4 { get; set; }
    public string dia_ent5 { get; set; }
    public string dia_ent6 { get; set; }
    public string dia_ent7 { get; set; }
    public string orden_ent { get; set; }
    public string tipo { get; set; }
    public string suc { get; set; }
    public string fec_alta { get; set; }
    public string hra_alta { get; set; }
    public string usr_alta { get; set; } //De la interfaz ITcausrService
    public string apartado_p { get; set; }
    public string status { get; set; }
    public string env_pub { get; set; }
    public string nom_com { get; set; }
    public string dir3 { get; set; }
    public string fec_lim_cre { get; set; }
    public string usr_lim_cre { get; set; }
    public string imp_aut_bonif { get; set; }
    public string rfc { get; set; }
    public string CURP { get; set; }
    public short DiasProntoPago2 { get; set; }
    public short DiasProntoPago3 { get; set; }
    public short DiasProntoPago4 { get; set; }
    public decimal DescuentoPPago2 { get; set; }
    public decimal DescuentoPPago3 { get; set; }
    public decimal DescuentoPPago4 { get; set; }
    public string Agrupacion1 { get; set; }
    public string Agrupacion2 { get; set; }
    public string Agrupacion3 { get; set; }
    public decimal PorcientoCargoReparto { get; set; }
    public string RegistroIEPS { get; set; }
    public string Fax2 { get; set; }
    public string Mail1 { get; set; }
    public string Mail2 { get; set; }
    public string Mail3 { get; set; }
    public string DirInternet { get; set; }
    public string Modulo { get; set; }
    public string ApellidoPaterno { get; set; }
    public string ApellidoMaterno { get; set; }
    public string Nombre { get; set; }
    public string ZonaCobro { get; set; }
    public string RegistroAlcohol { get; set; }
    public string num_int { get; set; }
    public string id1 { get; set; }
    public string id2 { get; set; }
    public string id3 { get; set; }
}

public class cxccli_ad : cxccli
{
    //NATIVAMENTE SON 9 CAMPOS
    //HEREDA 2 CAMPOS DE CXCCLI
    // Puedes acceder a la propiedad 'cia', 'cve', directamente, ya que la heredas de cxccli
    public string campo { get; set; }
    public int valor_int { get; set; }
    public float valor_float { get; set; }
    public string f_mod { get; set; }
    public string h_mod { get; set; }
    public string u_mod { get; set; } //De la interfaz ITcausrService
    public string ciaEnCxccli => this.cia;
    public string cveEnCxccli => this.cve;
    public string valor_str1EnCxccli => this.nom;
    public string valor_str1 { get; set; } //ESTE CAMPO ES LA SEGUNDA INSERCIÓN CON EL CATALOGO cceuso_ad
}

public class cxccli_sat : cxccli
{
    //NATIVAMENTE SON 9 CAMPOS
    //HEREDA 2 CAMPOS DE CXCCLI
    // Puedes acceder a la propiedad 'cia', 'cve' directamente, ya que la heredas de cxccli 
    public string usocfdi { get; set; }
    public string f_alta { get; set; }
    public string h_alta { get; set; }
    public string f_mod { get; set; }
    public string h_mod { get; set; }
    public string u_alta { get; set; }  //De la interfaz ITcausrService
    public string u_mod { get; set; }  //De la interfaz ITcausrService
    public string ciaEnCxccli => this.cia;
    public string cveEnCxccli => this.cve;
}

public class cxcdir : cxccli
{
    //NATIVAMENTE SON 24 CAMPOS
    //HEREDA 12 CAMPOS DE CXCCLI
    // Puedes acceder a la propiedad 'ibuff', 'cia', 'cve_cli', 'dir1', 'dir2', 'cd', 'est', 'pais', 'pos', 'tel1', 'tel2', 'fax' directamente, ya que la heredas de cxccli 
    public string num { get; set; }
    public string env_nom { get; set; }
    public string rep { get; set; }
    public string emb_ruta { get; set; }
    public string orden_ent { get; set; }
    public string f_mod { get; set; }
    public string h_mod { get; set; }
    public string Latitud { get; set; }
    public string Longitud { get; set; }
    public string Km { get; set; }
    public string IncluyeCartaPorte { get; set; }
   public string u_mod { get; set; }  //De la interfaz ITcausrService   
    public string ibuffEnCxccli => this.ibuff;
    public string ciaEnCxccli => this.cia;
    public string cve_cliEnCxccli => this.cve;
    public string dir1EnCxccli => this.dir1;
    public string dir2EnCxccli => this.dir2;
    public string cdEnCxccli => this.cd;
    public string estEnCxccli => this.est;
    public string paisEnCxccli => this.pais;
    public string posEnCxccli => this.pos;
    public string tel1EnCxccli => this.tel1;
    public string tel2EnCxccli => this.tel2;
    public string faxEnCxccli => this.fax;
}

public class cxcfpg : cxccli
{
    //NATIVAMENTE SON 5 CAMPOS
    //HEREDA 2 CAMPOS DE CXCCLI
    // Puedes acceder a la propiedad 'cia', 'Cliente', directamente, ya que la heredas de cxccli 
    public string Codigo { get; set; }
    public string Par { get; set; }
    public string Condicion { get; set; }
    public string CiaEnCxccli => this.cia;
    public string ClienteEnCxccli => this.cve;
}

public class cfrcli : cxccli
{
    //NATIVAMENTE SON 32 CAMPOS
    //HERADA 13 CAMPOS DE CXCCLI
    // Puedes acceder a la propiedad Cte, Nom, Dir1, Dir2, Cd, Edo, Pais, Cp, Tel1, Tel2, RFC, email, SegMer  directamente, ya que la heredas de cxccli 
    public string CodBarras { get; set; }
    public string TotalPuntos { get; set; }
    public string SaldoPuntos { get; set; }
    public string TotalMonedero { get; set; }
    public string SaldoMonedero { get; set; }
    public string TotalVenta { get; set; }
    public string TotalDsc { get; set; }
    public string TotalImpto { get; set; }
    public string NumVtas { get; set; }
    public string f_alt { get; set; }
    public string h_alt { get; set; }
    public string f_mod { get; set; }
    public string h_mod { get; set; }
    public string Id_Tarjeta { get; set; }
    public string SaldoMonederoCpr { get; set; }
    public string SaldoMonederoCer { get; set; }
    public string SaldoMonederoOtro { get; set; }
    public string u_alta { get; set; }  //De la interfaz ITcausrService
    public string u_mod { get; set; }  //De la interfaz ITcausrService
    public string CteEnCxccli => this.cve;
    public string NomEnCxccli => this.nom;
    public string Dir1EnCxccli => this.dir1;
    public string Dir2EnCxccli => this.dir2;
    public string CdEnCxccli => this.cd;
    public string EdoEnCxccli => this.est;
    public string PaisEnCxccli => this.pais;
    public string CpEnCxccli => this.pos;
    public string Tel1EnCxccli => this.tel1;
    public string Tel2EnCxccli => this.tel2;
    public string RFCEnCxccli => this.rfc;
    public string emailEnCxccli => this.Mail1;
    public string SegMerEnCxccli => this.seg_mer;

}