﻿using DOCOsoft.UserManagement.Infrastructure;
using DOCOsoft.UserManagement.Application;
using DOCOsoft.UserManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using DOCOsoft.UserManagement.Application.Users.Commands.CreateUser;
using DOCOsoft.UserManagement.Application.Behaviors;
using MediatR;
using DOCOsoft.UserManagement.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .Build();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly);
});

builder.Services.AddInfrastructure(configuration);
builder.Services.AddApplicationServices();



var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
