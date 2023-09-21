namespace AppClientesMexaba.Models
{
    public class ShopifyOrders
    {
        public List<ShopifyOrder> Orders { get; set; }

        public ShopifyOrders()
        {
            Orders = new List<ShopifyOrder>();
        }
    }
}
