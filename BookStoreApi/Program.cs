
using BookStoreApi.Application.Interfaces;
using BookStoreApi.Application.Services;
using BookStoreApi.Data;
using BookStoreApi.Middlewares;
using Microsoft.EntityFrameworkCore;
using BookStoreApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBooksService,BooksService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddDbContext<AppDbContext>(options=> options.UseInMemoryDatabase("BookDb"));
var app = builder.Build();
 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();