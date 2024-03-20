using bakery_asp.Services.Interfaces;
using bakery_asp_domain.Entities;
using bakery_asp_domain.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace bakery_asp.Services.Implementations
{
    public class ApiCategoryService : ICategoryService
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger _logger;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly HttpClient _httpClient;

        public ApiCategoryService(HttpClient httpClient, ILogger<ApiProductService> logger)
        {
            _httpClient = httpClient;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }
        public async Task<ResponseData<Category>> CreateCategoryAsync(Category product)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Categories");

            var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data = await response
                    .Content.ReadFromJsonAsync<ResponseData<Category>>(_serializerOptions);

                return data; // product;
            }
            _logger.LogError($"-----> object not created. Error:{response.StatusCode.ToString()}");

            return new ResponseData<Category>
            {
                Success = false,
                ErrorMessage = $"Категория не добавлена. Error:{response.StatusCode.ToString()}"
            };
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + $"Categories/{id}");

            var response = await _httpClient.DeleteAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> object not deleted. Error:{response.StatusCode.ToString()}");
                throw new Exception($"Категория не удалена. Error:{response.StatusCode.ToString()}");
            }
        }

        public async Task<ResponseData<Category>> GetCategoryByIdAsync(int id)
        {
            // подготовка URL запроса
            var urlString = $"{_httpClient.BaseAddress!.AbsoluteUri}categories/{id}";

            // отправить запрос к API
            var response = await _httpClient.GetAsync(new Uri(urlString));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Category>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<Category>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }

            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");

            return new ResponseData<Category>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode.ToString()}"
            };
        }

        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            // подготовка URL запроса
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}categories/");
            
            // отправить запрос к API
            var response = await _httpClient.GetAsync(
                new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response
                    .Content
                    .ReadFromJsonAsync<ResponseData<List<Category>>>
                    (_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<List<Category>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");

            return new ResponseData<List<Category>>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode.ToString()}"
            };
        }

        public async Task UpdateCategoryAsync(int id, Category product)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + $"Categories/{id}");

            var response = await _httpClient.PutAsJsonAsync(uri, product, _serializerOptions);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> object not updated. Error:{response.StatusCode.ToString()}");
                throw new Exception($"Объект не обновлен. Error:{response.StatusCode.ToString()}");
            }
        }
    }
}
