using AppClientesMexaba.Servicios;

namespace AppClientesMexaba.Models
{
    public interface IClienteInserter
    {
        void InsertarCliente(cxccli cliente, string ciaVentas, BaseDbContext dbContext, string usuario);
        void InsertarCliente(cxccli_ad cliente, string ciaVentas, BaseDbContext dbContext, string usuario);
        void InsertarCliente(cxccli_sat cliente, string ciaVentas, BaseDbContext dbContext, string usuario);
        void InsertarCliente(cxcdir cliente, string ciaVentas, BaseDbContext dbContext, string usuario);
        void InsertarCliente(cxcfpg cliente, string ciaVentas, BaseDbContext dbContext, string usuario);
        void InsertarCliente(cfrcli cliente, string ciaVentas, BaseDbContext dbContext, string usuario);
    }

    

}
