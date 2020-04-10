using System.ComponentModel.DataAnnotations;

namespace InMemoryApi.Data
{
    public class Book
    {
        [Key]
        public string ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
    }
}