using UserApp.DataLayer;
using UserApp.DataLayer.Entities;
using BusinessLayer.Services;
using BusinessLayer.Interfaces.Services;
using BusinessLayer.Interfaces.Repository;
using BusinessLayer.Repository;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        
        builder.Services.AddControllersWithViews();
        builder.Services.AddEndpointsApiExplorer();   
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "DominikPatakov_4D_aspnet_2 API",
                Version = "v1"
            });
        });

        builder.Services.AddDbContext<AppDbContext>();

      
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddScoped<IItemService, ItemService>();
        builder.Services.AddScoped<IItemRepository, ItemRepository>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

        builder.Services.AddCors(option =>
        {
            option.AddPolicy("ReactLocalhost", policy =>
            policy
            .WithOrigins("http://localhost:5174", "https://localhost:5174")
            .AllowAnyHeader()
            .AllowAnyMethod()
            );
        });

        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        var app = builder.Build();

        
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        else
        {
            
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseCors("ReactLocalHost");
        app.UseSession();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();

            if (!db.Items.Any())
            {
                db.Items.AddRange(new ItemEntity
                {
                    ItemId = Guid.NewGuid(),
                    PublicId = Guid.NewGuid(),
                    Name = "Acme Coffee Mug",
                    Description = "Durable ceramic mug with logo",
                    Price = 9.99m,
                    StockQuantity = 50
                },
                new ItemEntity
                {
                    ItemId = Guid.NewGuid(),
                    PublicId = Guid.NewGuid(),
                    Name = "Wireless Mouse",
                    Description = "Ergonomic wireless mouse",
                    Price = 24.99m,
                    StockQuantity = 25
                },
                new ItemEntity
                {
                    ItemId = Guid.NewGuid(),
                    PublicId = Guid.NewGuid(),
                    Name = "Notebook",
                    Description = "A5 lined notebook, 200 pages",
                    Price = 4.49m,
                    StockQuantity = 100
                });

                db.SaveChanges();
            }
        }

        await app.RunAsync();
    }
}