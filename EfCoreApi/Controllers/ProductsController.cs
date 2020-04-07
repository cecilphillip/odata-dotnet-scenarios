using System.Linq;
using EfCoreApi.Data;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EfCoreApi.Controllers
{
    [ODataRoutePrefix("products")]
    [ApiController]
    public class ProductsController : ODataController
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly ProductsContext context;

        public ProductsController(ProductsContext context, ILogger<ProductsController> logger)
        {
            this.context = context;
            _logger = logger;
        }

        [HttpGet("")]
        [ODataRoute]
        [EnableQuery]
        public ActionResult<IQueryable<Product>> RetrieveArtifactsQueryable()
        {
            var records = context.Products;
            return Ok(records);
        }
    }
}
