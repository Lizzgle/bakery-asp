using bakery_asp_api.Data;
using bakery_asp_api.Services.Interfaces;
using bakery_asp_domain.Entities;
using bakery_asp_domain.Models;
using Microsoft.EntityFrameworkCore;

namespace bakery_asp_api.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly DbSet<Product> _products;
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context) 
        {
            _products = context.Products;
            _context = context;
        }
        public async Task<ResponseData<Product>> CreateProductAsync(Product product)
        {
            await _products.AddAsync(product);

            _context.SaveChanges();

            return new ResponseData<Product>() {Data = product};
        }

        public async Task DeleteProductAsync(int id)
        {
            Product? product = await _products
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product != null)
            {
                _products.Remove(product);
                _context.SaveChanges();
            }
        }

        public async Task<ResponseData<Product>> GetProductByIdAsync(int id)
        {
            Product? product = await _products
                .FirstOrDefaultAsync(x => x.Id == id);

            ResponseData<Product> response = new();

            if (product != null)
            {
                response.Data = product;
            }
            else
            {
                response.ErrorMessage = "Product not found";
                response.Success = false;
            }
            return response;
        }

        public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryName)
        {
            IQueryable<Product> products = _products
                .Include(p => p.Category)
                .AsQueryable();
            if (!string.IsNullOrEmpty(categoryName))
            {
                products = products.Where(p => p.Category!.Name!.Equals(categoryName));
            }

            ListModel<Product> listModel = new();
            foreach (Product product in products)
            {
                listModel.Items.Add(product);
            }
            return new ResponseData<ListModel<Product>>() { Data = listModel };
        }

        public async Task UpdateProductAsync(int id, Product product)
        {
            Product? elem = await _products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (elem != null)
            {
                elem.Name = product.Name;
                elem.Description = product.Description;
                elem.Price = product.Price;
                elem.Category = product.Category;
                elem.Calories = product.Calories;
                _context.SaveChanges();
            }
        }
    }
}
