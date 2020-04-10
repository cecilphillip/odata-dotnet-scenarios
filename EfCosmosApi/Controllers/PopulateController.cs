using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using EfCosmosApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Cosmos;

namespace EfCosmosApi.Controllers
{
    [ApiController]
    [Route("/api/populate")]
    public class PopulateController : ControllerBase
    {
        private readonly ProductsContext context;

        public PopulateController(ProductsContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Run()
        {
            // Remote all records from the collection
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            // populate catalog
            var data = Generate();
            foreach (var item in data)
            {
                context.Add(item);
                await context.SaveChangesAsync();
            }
            return StatusCode(StatusCodes.Status201Created);
        }

        static List<Product> Generate() =>
             new Faker<Product>()
               .RuleFor(p => p.ID, f => Guid.NewGuid().ToString("N"))
               .RuleFor(p => p.Name, f => f.Name.FullName())
               .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
               .RuleFor(p => p.BarCode, f => f.Commerce.Ean13())
               .RuleFor(p => p.Quantity, f => f.Random.Number(1, 200))
               .RuleFor(p => p.Price, f =>
               {
                   decimal amount = 150.0m - 10.0m;
                   var part = (decimal)f.Random.Double() * amount;
                   return Math.Round(10 + part, 2);
               })
               .Generate(100);
    }
}