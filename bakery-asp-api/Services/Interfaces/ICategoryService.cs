using bakery_asp_domain.Entities;
using bakery_asp_domain.Models;

namespace bakery_asp_api.Services.Interfaces
{
    public interface ICategoryService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }

}
