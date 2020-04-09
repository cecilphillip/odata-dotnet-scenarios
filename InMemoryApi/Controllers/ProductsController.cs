using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;

namespace InMemoryApi.Controllers
{
    [ODataRoutePrefix("products")]
    [ApiController]
    public class ProductsController : ODataController
    {
        private static List<Product> Products { get; set; }

        static ProductsController()
        {
            var products = new Faker<Product>()
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

            Products = products;
        }

        [ODataRoute]
        [EnableQuery]
        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok(Products.AsQueryable());
        }
    }
}
