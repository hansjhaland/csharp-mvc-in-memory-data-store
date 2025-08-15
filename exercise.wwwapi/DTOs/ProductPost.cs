using Microsoft.AspNetCore.Hosting.Server;

namespace exercise.wwwapi.DTOs
{
    public class ProductPost
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
    }
}
