using AppClientesMexaba.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppClientesMexaba.Data
{
    public class D_ShopifyOrders
    {
        private readonly HttpClient _httpClient;

        public D_ShopifyOrders(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://mexicana-de-abarrotes.myshopify.com"); // Reemplaza con la URL de tu tienda Shopify
        }

        public async Task<List<ShopifyOrders>> GetOrdersAsync(string apiKey, string apiPassword, string shopifyStoreUrl)
        {
            try
            {
                string authHeader = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"ba19d691bba6a53c770bd145bf3c7c7f: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"));
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {authHeader}");

                // Ajusta la URL base de la tienda Shopify en el constructor, no es necesario concatenarla aquí
                string ordersApiUrl = "/admin/api/2023-07/orders.json"; // Puedes ajustar la versión de la API si es necesario

                HttpResponseMessage response = await _httpClient.GetAsync(ordersApiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var orders = JsonConvert.DeserializeObject<ShopifyOrdersResponse>(responseBody);
                    return orders.Orders;
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones personalizado
                throw new ShopifyApiException("Error al obtener los pedidos de Shopify", ex);
            }
        }

    }

    public class ShopifyOrdersResponse
    {
        public List<ShopifyOrders> Orders { get; set; }
    }

    public class ShopifyOrders
    {
        // Define propiedades que coincidan con la estructura de los pedidos en la respuesta JSON
    }

    public class ShopifyApiException : Exception
    {
        public ShopifyApiException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
