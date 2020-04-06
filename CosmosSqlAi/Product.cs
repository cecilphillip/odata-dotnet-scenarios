using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CosmosSqlAi
{
    public class Product
    {
        [Key]
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("barcode")]
        public string BarCode { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
