using System;
using System.Linq;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;

namespace CosmosSqlAi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddOData();
            services.AddSingleton<CosmosClient>(provider =>
            {
                var config = provider.GetService<IConfiguration>();
                return new CosmosClientBuilder(config["COSMOSCONNECTION"])
                        .WithApplicationName(nameof(CosmosSqlAi))
                        .WithRequestTimeout(TimeSpan.FromSeconds(2))
                        .Build();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapODataRoute("ODataRoute", "odata", GetEdmModel())
                    .Select().Filter().Count().MaxTop(50).OrderBy().SkipToken();
            });
        }

        public static IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            var artifactEntitySet = odataBuilder.EntitySet<Product>("Products");
            return odataBuilder.GetEdmModel();
        }
    }
}
