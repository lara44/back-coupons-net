using back_coupons.Data;
using back_coupons.Repositories.Implementations;
using back_coupons.Repositories.Interfaces;
using back_coupons.UnitsOfWork.Implementations;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=LocalConnection"));
builder.Services.AddTransient<SeedDB>();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("VueCorsPolicy",
        builder =>
        {
            builder.WithOrigins("*")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Container UnitOfWork
builder.Services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();
builder.Services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));
builder.Services.AddScoped<IProductUnitOfWork, ProductUnitOfWork>();
builder.Services.AddScoped<ICountryUnitOfWork, CountryUnitOfWork>();
builder.Services.AddScoped<IStateUnitOfWork, StateUnitOfWork>();
builder.Services.AddScoped<ICompanyUnitOfWork, CompanyUnitOfWork>();
// Container Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();



var app = builder.Build();

SeedData(app);

void SeedData(WebApplication app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory!.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<SeedDB>();
        service!.SeedAsync().Wait();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("VueCorsPolicy");

app.Run();
