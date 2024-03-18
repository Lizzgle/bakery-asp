using bakery_asp_domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace bakery_asp_api.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Выполнение миграций
            await context.Database.MigrateAsync();

            Category[] categories = new Category[]
            {
                new Category {Name="cupcake"},
                    new Category {Name="biscuit"},
                    new Category {Name="bread"},
                    new Category {Name="pastries"}
            };
            context.Categories.AddRange(categories);


            await context.AddRangeAsync(new Product[]
            {
                new Product() { Name = "Cookies with chocolate",
                                  Price = 7.59, Category = categories.First(c => c.Name!.Equals("biscuit")) },
                new Product() { Name = "Cookies with cherry",
                                  Price = 7.59, Category = categories.First(c => c.Name!.Equals("biscuit")) },
                

                new Product() { Name = "croissant",
                                  Price = 5.99, Category = categories.First(c => c.Name!.Equals("pastries")) },
                new Product() { Name = "puff pastry",
                                  Price = 5.99, Category = categories.First(c => c.Name!.Equals("pastries")) },
                
            });

            await context.SaveChangesAsync();
        }
    }
}
