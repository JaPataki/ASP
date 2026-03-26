using BusinessLayer.Interfaces.Repository;
using BusinessLayer.Interfaces.Services;
using BusinessLayer.Repository;
using BusinessLayer.Services;
using UserApp.DataLayer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>();



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IItemService, ItemService>();    
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddCors(option =>
{
    option.AddPolicy("ReactLocalHost", policy =>
    policy
    .WithOrigins("https://localhost:5174", "https://localhost:5174")
    .AllowAnyHeader()
    .AllowAnyMethod()
    );
});



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapGet("/", () => Results.Redirect("/swagger"));
}


app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("ReactLocalHost");
app.UseSession();

app.UseAuthorization();

app.MapControllers();

app.Run();
