using bakery_asp_api.Services.Interfaces;
using bakery_asp_domain.Entities;
using bakery_asp_domain.Models;
using Microsoft.EntityFrameworkCore;

namespace bakery_asp_api.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly DbSet<Category> _categories;

        public CategoryService(DbSet<Category> categories)
        {
            _categories = categories;
        }
        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            ResponseData<List<Category>> response = new();
            try
            {
                response.Data = await _categories.ToListAsync();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }
    }
}
