﻿using Blog.Api;
using Blog.Core;
using Blog.Core.Domain.Identity;
using Blog.Core.SeedWords;
using Blog.Data.Repositories;
using Blog.Data.SeedWords;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection");//Connect databasse

//Config DB Context and ASP.net core identity
builder.Services.AddDbContext<BlogContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<BlogContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    //Password settings
    options.Password.RequireDigit = true; // số
    options.Password.RequireLowercase = true; //thường
    options.Password.RequireNonAlphanumeric = true; //ký tự đặc biệc
    options.Password.RequireUppercase = true; //hoa
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    //Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);//sau bao lâu sẽ tự logout
    options.Lockout.MaxFailedAccessAttempts = 5; //Số lần login fail bị khóa
    options.Lockout.AllowedForNewUsers = true;

    //User settings
    options.User.AllowedUserNameCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()";
    options.User.RequireUniqueEmail = false;
});

// Add services to the container.

builder.Services.AddScoped(typeof(IReadOnlyDictionary<,>), typeof(RepositotyBase<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Business services and repositories
var services = typeof(PostRepository).Assembly.GetTypes()
    .Where(x => x.GetInterfaces().Any(i => i.Name == typeof(IRepository<,>).Name)
    && !x.IsAbstract && x.IsClass && !x.IsGenericType);

foreach (var service in services)
{
    var allInterfaces = service.GetInterfaces();
    var direcInterface = allInterfaces.Except(allInterfaces.SelectMany(t => t.GetInterfaces())).FirstOrDefault();
    if (direcInterface != null)
    {
        builder.Services.Add(new ServiceDescriptor(direcInterface, service, ServiceLifetime.Scoped));
    }
}

//Default config to the container
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

//Seeding data
app.MigrationDatabase();

app.Run();