using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoApi.Controllers
{
    [ApiController]
    [Route("/api/populate")]
    public class PopulateController : ControllerBase
    {
        private readonly IMongoClient mongoClient;
        private readonly ILogger<PopulateController> logger;
        public PopulateController(IMongoClient mongoClient, ILogger<PopulateController> logger)
        {
            this.logger = logger;
            this.mongoClient = mongoClient;
        }

        [HttpPost("")]
        public async Task<ActionResult> Run()
        {
            var db = mongoClient.GetDatabase("products");
            var collection = db.GetCollection<Product>("catalog");

            // Remote all records from the collection
            await collection.DeleteManyAsync(p => p.ID != string.Empty);

            var data = Generate();

            // populate catalog
            foreach (var item in data)
            {
                try
                {
                    await collection.InsertOneAsync(item);
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex, "There was an issue with");
                }
            }

            // InsertManyAsync fails depending on available throughput
            // await collection.InsertManyAsync(data, new InsertManyOptions
            // {
            //     IsOrdered = false
            // });

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