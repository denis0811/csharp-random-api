// Program.cs - This file sets up and runs your ASP.NET Core Web Application.
// It includes the necessary configurations for routing and Swagger/OpenAPI.

using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc; // Required for [ApiController] and [HttpGet]

// Define the namespace for your application.
// This should match your project's root namespace (e.g., the folder your .csproj is in).
namespace RandomNumberApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create the web application builder.
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // This configures services like controllers, Swagger, etc.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer(); // Enables API explorer for Swagger
            builder.Services.AddSwaggerGen(); // Generates Swagger documentation

            // Build the application.
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // This sets up middleware for handling requests.
            if (app.Environment.IsDevelopment())
            {
                // In development, enable Swagger UI for API testing.
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Enable HTTPS redirection in production (good practice, though Render handles SSL).
            app.UseHttpsRedirection();

            // Enable authorization middleware (if you were using authentication/authorization).
            // app.UseAuthorization();

            // Map controller routes. This tells the app to use controllers for routing.
            app.MapControllers();

            // Run the application.
            app.Run();
        }
    }

    // Define the API controller.
    // [ApiController] attribute enables API-specific behaviors like automatic model validation.
    // [Route] attribute defines the base route for the controller.
    [ApiController]
    [Route("[controller]")] // This means the route will be /randomnumbers
    public class RandomNumbersController : ControllerBase
    {
        private readonly Random _random = new Random();

        /// <summary>
        /// Generates a distinct list of 8 random integers between 1 and 99.
        /// </summary>
        /// <returns>A list of 8 distinct integers.</returns>
        [HttpGet] // This defines an HTTP GET endpoint.
        public ActionResult<IEnumerable<int>> Get()
        {
            // Use a HashSet to ensure distinct numbers.
            var distinctNumbers = new HashSet<int>();
            const int count = 8; // Number of integers to generate
            const int min = 1;   // Minimum value (inclusive)
            const int max = 99;  // Maximum value (inclusive)

            // Loop until we have generated the required number of distinct integers.
            while (distinctNumbers.Count < count)
            {
                // Generate a random number within the specified range.
                // Next(minValue, maxValue) generates a number between minValue (inclusive)
                // and maxValue (exclusive), so we use max + 1.
                int randomNumber = _random.Next(min, max + 1);

                // Attempt to add the number to the HashSet.
                // Add() returns true if the element was added (i.e., it was distinct),
                // false if it was already present.
                distinctNumbers.Add(randomNumber);
            }

            // Convert the HashSet to a List and return it as an HTTP 200 OK response.
            return Ok(distinctNumbers.ToList());
        }
    }
}
