using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using InMemoryApi.Data;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;

namespace InMemoryApi.Controllers
{
    [ODataRoutePrefix("books")]
    [ApiController]
    public class BooksController : ODataController
    {
        private static List<Book> Books { get; set; }

        static BooksController()
        {
            var books = new Faker<Book>()
                .RuleFor(p => p.ID, f => Guid.NewGuid().ToString("N"))
                .RuleFor(p => p.Title, f => f.Commerce.ProductName())
                .RuleFor(p => p.Author, f => f.Name.FullName())
                .RuleFor(p => p.ISBN, f => f.Commerce.Ean13())
                .RuleFor(p => p.Genre, f => f.Commerce.Categories(1)[0])
                .RuleFor(p => p.Price, f =>
                {
                    decimal amount = 150.0m - 10.0m;
                    var part = (decimal)f.Random.Double() * amount;
                    return Math.Round(10 + part, 2);
                })
                .Generate(100);

            Books = books;
        }

        [ODataRoute]
        [EnableQuery]
        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok(Books.AsQueryable());
        }
    }
}