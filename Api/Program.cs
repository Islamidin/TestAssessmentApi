using System.Reflection;
using Api.Services;
using Database;
using Database.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
                                                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPeopleService, PeopleService>();
builder.Services.AddScoped<IPeopleRepository, PeopleRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(cfg => { cfg.AllowNullCollections = true; }, Assembly.GetExecutingAssembly());
builder.Services.AddValidatorsFromAssemblies([Assembly.GetAssembly(typeof(Program))]);
builder.Services.AddFluentValidationAutoValidation(config => { config.EnableFormBindingSourceAutomaticValidation = true; });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();