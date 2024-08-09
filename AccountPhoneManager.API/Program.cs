using AccountPhoneManager.Core.Abstraction;
using AccountPhoneManager.Core.Repositories;
using AccountPhoneManager.Core.Services;
using AccountPhoneManager.Core.Validators;
using AccountPhoneManager.DAL.Contexts;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<AccountManagerDbContexts>(options =>
{
    options.UseSqlServer(connectionString);
});
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountManagmentService, AccountManagmentService>();
builder.Services.AddScoped<IPhoneRepository, PhoneRepository>();
builder.Services.AddScoped<IPhoneManagmentService, PhoneManagmentService>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configure System.Text.Json to handle reference loops
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<PhoneNumberValidator>();
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
