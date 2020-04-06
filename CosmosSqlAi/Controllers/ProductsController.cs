using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace CosmosSqlAi.Controllers
{
    [ODataRoutePrefix("products")]
    [ApiController]
    public class ProductsController : ODataController
    {
        private CosmosClient cosmos { get; }

        public ProductsController(CosmosClient cosmos)
        {
            this.cosmos = cosmos;
        }

        [HttpGet("")]
        [ODataRoute]
        [EnableQuery]
        public ActionResult<IQueryable<Product>> RetrieveArtifactsQueryable()
        {
            var records = Products.AsQueryable();
            return Ok(records);
        }
    }
}
