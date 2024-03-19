using bakery_asp_api.Data;
using bakery_asp_api.Services.Interfaces;
using bakery_asp_domain.Entities;
using bakery_asp_domain.Models;
using Microsoft.EntityFrameworkCore;

namespace bakery_asp_api.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly DbSet<Category> _categories;
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _categories = context.Categories;
            _context = context;
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

        public async Task<ResponseData<Category>> CreateCategoryAsync(Category category)
        {
            await _categories.AddAsync(category);

            _context.SaveChanges();
            return new ResponseData<Category>() { Data = category };
        }

        public async Task DeleteCategoryAsync(int id)
        {
            Category? category = await _categories
                .FirstOrDefaultAsync(x => x.Id == id);

            if (category != null)
            {
                _categories.Remove(category);
                _context.SaveChanges();
            }
        }

        //public async Task DeleteCategoryAsync(int id)
        //{
        //    Category? category = await _categories
        //        .FirstOrDefault(c => c.Id == id);
        //    if (category != null)
        //    {
        //        _categories.Remove(category);
        //        _context.SaveChanges();
        //    }
        //}

        public async Task<ResponseData<Category>> GetCategoryByIdAsync(int id)
        {
            Category? category = await _categories
                .FirstOrDefaultAsync(x => x.Id == id);

            ResponseData<Category> response = new();

            if (category != null)
                response.Data = category;
            else
            {
                response.Success = false;
                response.ErrorMessage = "Category not found";
            }
            return response;
        }



        public async Task UpdateCategoryAsync(int id, Category category)
        {
            Category? elem = await _categories
               .FirstOrDefaultAsync(x => x.Id == id);
            if (category != null)
            {
                elem!.Name = category.Name;
                _context.SaveChanges();
            }
        }
    }
}
