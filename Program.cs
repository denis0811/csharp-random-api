// Program.cs - This file sets up and runs your ASP.NET Core Web Application.
// It includes the necessary configurations for routing, Swagger/OpenAPI, and CORS.

using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// Define the namespace for your application.
namespace RandomNumberApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // AddControllers registers controller-based services.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure CORS (Cross-Origin Resource Sharing)
            // This allows your web UI (running on a different domain) to access this API.
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        // In a production scenario, you would replace this with the specific URL of your UI
                        // For example: .WithOrigins("https://webuirandomnumbersapi.onrender.com")
                        // For simplicity, we'll allow all origins for now.
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Use the CORS policy you defined. This must be placed after UseRouting and before MapControllers.
            app.UseCors();

            app.MapControllers();

            app.Run();
        }
    }

    // Define the API controller.
    [ApiController]
    [Route("[controller]")]
    public class RandomNumbersController : ControllerBase
    {
        private readonly Random _random = new Random();

        /// <summary>
        /// Generates a distinct list of 8 random integers between 1 and 99.
        /// </summary>
        /// <returns>A list of 8 distinct integers.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<int>> Get()
        {
            return Ok(GenerateDistinctRandomNumbers(8));
        }

        /// <summary>
        /// Generates a distinct list of random integers, with the total number specified.
        /// </summary>
        /// <param name="count">The number of random integers to generate (max 100).</param>
        /// <returns>A list of distinct integers.</returns>
        [HttpGet("{count}")]
        public ActionResult<IEnumerable<int>> Get(int count)
        {
            if (count < 0 || count > 100)
            {
                return BadRequest("Count must be between 0 and 100.");
            }
            return Ok(GenerateDistinctRandomNumbers(count));
        }

        private IEnumerable<int> GenerateDistinctRandomNumbers(int count)
        {
            var distinctNumbers = new HashSet<int>();
            const int min = 1;
            const int max = 99;

            while (distinctNumbers.Count < count)
            {
                int randomNumber = _random.Next(min, max + 1);
                distinctNumbers.Add(randomNumber);
            }
            return distinctNumbers.ToList();
        }
    }
}
