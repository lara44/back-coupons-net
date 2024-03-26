using back_coupons.Data;
using back_coupons.Repositories.Implementations;
using back_coupons.Repositories.Interfaces;
using back_coupons.UnitsOfWork.Implementations;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=LocalConnection"));

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
builder.Services.AddScoped<IContactUnitOfWork, ContactUnitOfWork>();
builder.Services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));
// Container Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));



var app = builder.Build();

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
