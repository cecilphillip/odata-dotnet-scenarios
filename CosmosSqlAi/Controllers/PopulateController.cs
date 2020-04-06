using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace CosmosSqlAi.Controllers
{
    [ApiController]
    [Route("api/populate")]
    public class PopulateController : ControllerBase
    {
        private const string DATABASE_ID = "products";
        private const string CONTAINER_NAME = "catalog";
        private CosmosClient cosmos;
        private Container container;
        private ILogger<PopulateController> logger;

        public PopulateController(CosmosClient cosmos, ILogger<PopulateController> logger)
        {
            this.logger = logger;
            this.cosmos = cosmos;
            this.container = cosmos.GetContainer(DATABASE_ID, CONTAINER_NAME);
        }

        [HttpPost]
        public async Task<ActionResult> Run()
        {
            var items = Generate();
            List<Task> tasks = new List<Task>();

            foreach (var item in items)
            {
                tasks.Add(container.CreateItemAsync<Product>(item, new PartitionKey(item.ID))
                    .ContinueWith(responseTask =>
                    {
                        if (responseTask.IsCompletedSuccessfully) return;

                        AggregateException innerExceptions = responseTask.Exception.Flatten();
                        var exception = innerExceptions.InnerExceptions.FirstOrDefault();
                        switch (exception)
                        {
                            case CosmosException cosmosException:
                                logger.LogError(cosmosException, "Error inserting documents");
                                break;
                            case Exception ex:
                                logger.LogError(ex, "An unexpected exception was thrown");
                                break;
                            default:
                                logger.LogError("An unexpected exception was thrown");
                                break;
                        }
                    }));
            }

            await Task.WhenAll(tasks);
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