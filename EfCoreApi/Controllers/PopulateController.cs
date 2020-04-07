using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using EFCore.BulkExtensions;
using EfCoreApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfCoreApi.Controllers
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
            //await context.Database.ExecuteSqlRawAsync("DELETE TABLE Products;");

            // populate catalog
            await context.BulkInsertAsync(Generate());
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