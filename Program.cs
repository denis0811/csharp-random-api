// Program.cs - This file sets up and runs your ASP.NET Core Web Application.
// It includes the necessary configurations for routing and Swagger/OpenAPI.

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

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
            // Ensure the count is not negative and not greater than a reasonable limit.
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
