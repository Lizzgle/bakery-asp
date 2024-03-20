using bakery_asp;
using bakery_asp.Services.Implementations;
using bakery_asp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();

builder.Services.AddHttpClient<IProductService, ApiProductService>(opt =>
                                            opt.BaseAddress = new Uri(uriData!.ApiUri));

builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
                                            opt.BaseAddress = new Uri(uriData!.ApiUri));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
