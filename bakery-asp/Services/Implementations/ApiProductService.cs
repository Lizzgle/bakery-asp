using bakery_asp.Services.Interfaces;
using bakery_asp_domain.Entities;
using bakery_asp_domain.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace bakery_asp.Services.Implementations
{
    public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiProductService> _logger;
        private readonly JsonSerializerOptions _serializerOptions;

        public ApiProductService(HttpClient httpClient, ILogger<ApiProductService> logger)
        {
            _httpClient = httpClient;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }

        public async Task<ResponseData<Product>> CreateProductAsync(Product product)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Products");

            var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data = await response
                    .Content.ReadFromJsonAsync<ResponseData<Product>>(_serializerOptions);

                return data; // product;
            }
            _logger.LogError($"-----> object not created. Error:{ response.StatusCode.ToString()}");
            
            return new ResponseData<Product>
            {
                Success = false,
                ErrorMessage = $"Объект не добавлен. Error:{ response.StatusCode.ToString() }"
            };
        }

        public async Task DeleteProductAsync(int id)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + $"Products/{id}");

            var response = await _httpClient.DeleteAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> object not deleted. Error:{response.StatusCode.ToString()}");
                throw new Exception($"Объект не удален. Error:{response.StatusCode.ToString()}");
            }
        }

        public async Task<ResponseData<Product>> GetProductByIdAsync(int id)
        {
            // подготовка URL запроса
            var urlString = $"{_httpClient.BaseAddress!.AbsoluteUri}products/{id}";

            // отправить запрос к API
            var response = await _httpClient.GetAsync(new Uri(urlString));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Product>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<Product>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }

            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");

            return new ResponseData<Product>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode.ToString()}"
            };
        }


        public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryName)
        {
            // подготовка URL запроса
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}products/");
            // добавить категорию в маршрут
            if (categoryName != null)
            {
                urlString.Append($"{categoryName}/");
            };
            // отправить запрос к API
            var response = await _httpClient.GetAsync(
                new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response
                    .Content
                    .ReadFromJsonAsync<ResponseData<ListModel<Product>>>
                    (_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<ListModel<Product>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: { response.StatusCode.ToString()}");
            
            return new ResponseData<ListModel<Product>> 
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: { response.StatusCode.ToString() }"
            };
        }


        public async Task UpdateProductAsync(int id, Product product)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + $"Products/{id}");

            var response = await _httpClient.PutAsJsonAsync(uri, product, _serializerOptions);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> object not updated. Error:{response.StatusCode.ToString()}");
                throw new Exception($"Объект не обновлен. Error:{response.StatusCode.ToString()}");
            }
        }
    }
}
