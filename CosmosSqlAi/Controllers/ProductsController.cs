using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace CosmosSqlAi.Controllers
{
    [ODataRoutePrefix("products")]
    [ApiController]
    public class ProductsController : ODataController
    {
        private const string DATABASE_ID = "products";
        private const string CONTAINER_NAME = "catalog";
        private CosmosClient cosmos;
        private Container container;

        public ProductsController(CosmosClient cosmos)
        {
            this.cosmos = cosmos;
            this.container = cosmos.GetContainer(DATABASE_ID, CONTAINER_NAME);
        }

        [HttpGet("")]
        [ODataRoute]
        [EnableQuery]
        public ActionResult<IQueryable<Product>> RetrieveArtifactsQueryable()
        {
            // Need to set allowSynchronousQueryExecution true ☹️
            var records = container.GetItemLinqQueryable<Product>(allowSynchronousQueryExecution: true);
            return Ok(records);
        }
    }
}
