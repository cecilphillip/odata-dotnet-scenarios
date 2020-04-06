using System.Linq;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MongoApi
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

            BsonClassMap.RegisterClassMap<Product>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(p => p.ID);
            });

            services.AddSingleton<IMongoClient>(provider =>
            {
                var config = provider.GetService<IConfiguration>();
                return new MongoClient(config["MONGOCONNECTION"]);
            });

            services.AddControllers();
            services.AddOData();
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
