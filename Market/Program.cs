using Market.Authentication;
using Market.DAL;
using Market.DAL.Repositories;
using Market.Modules.Carts;
using Market.Modules.Orders;

static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
{
    services.AddDbContext<RepositoryContext>();

    services.AddScoped<ICartsRepository, CartsRepository>();
    services.AddScoped<IOrdersRepository, OrdersRepository>();
    services.AddScoped<IProductsRepository, ProductsRepository>();
    services.AddScoped<IUsersRepository, UsersRepository>();
    services.AddScoped<IPasswordHasher, Sha256PasswordHasher>();

    services.AddScoped<AuthenticationFilter>();
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
// app.UseMiddleware<AuthenticationMiddleware>();
app.MapControllers();

app.Run();

