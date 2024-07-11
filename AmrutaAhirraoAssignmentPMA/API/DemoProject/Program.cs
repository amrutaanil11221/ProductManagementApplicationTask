
using DemoProject.Context;
using DemoProject.Interface;
using DemoProject.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace DemoProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder => builder
                        //.WithOrigins("http://localhost:4200/", "https://localhost:44377/api/Products")
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ProductConnectionString")));

            builder.Services.AddScoped<IProductServices, ProductServices>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors();
            app.UseHttpsRedirection();


            app.MapControllers();

            app.Run();
           
        }
    }
}
