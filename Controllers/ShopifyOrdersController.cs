using AppClientesMexaba.Data;
using Microsoft.AspNetCore.Mvc;

namespace AppClientesMexaba.Controllers
{
    public class ShopifyOrdersController : Controller
    {
        private readonly D_ShopifyOrders _shopifyOrdersService;


        public ShopifyOrdersController(D_ShopifyOrders shopifyOrdersService)
        {
            _shopifyOrdersService = shopifyOrdersService;
        }


        public async Task<IActionResult> Index()
        {
            var apiKey = "ba19d691bba6a53c770bd145bf3c7c7f";
            var apiPassword = "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";
            var shopifyStoreUrl = "https://mexicana-de-abarrotes.myshopify.com";

            var orders = await _shopifyOrdersService.GetOrdersAsync(apiKey, apiPassword, shopifyStoreUrl);
            return View(orders);
        }


    }
}
