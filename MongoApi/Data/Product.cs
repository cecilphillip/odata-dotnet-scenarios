using System.ComponentModel.DataAnnotations;

namespace MongoApi.Data
{
    public class Product
    {
        [Key]
        public string ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string BarCode { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
