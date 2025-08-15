
using exercise.wwwapi.DTOs;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Endpoints
{
    public static class ProductEndpoints
    {
        public static void ConfigureProduct(this WebApplication app)
        {
            var products = app.MapGroup("products");

            products.MapGet("/", GetProducts);
            products.MapGet("/{id}", GetProductById);
            products.MapPost("/", AddProduct);
            products.MapDelete("/{id}", Delete);
            products.MapPut("/{id}", Update);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetProducts(IRepository repository, [FromQuery] string category = "")
        {
            var results = await repository.GetAsync(category);
            return results.Count != 0 ? TypedResults.Ok(results) : TypedResults.NotFound(new { Error = "No products of the provided category were found" });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetProductById(IRepository repository, int id)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity == null) return TypedResults.NotFound(new { Error = "Product not found" });
            return TypedResults.Ok(entity);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public static async Task<IResult> AddProduct(IRepository repository, ProductPost model)
        {
            Product entity = new Product();
            entity.Name = model.Name;
            entity.Category = model.Category;
            entity.Price = model.Price;
            var results = await repository.AddAsync(entity);

            return TypedResults.Created($"https://localhost:7188/products/{entity.Id}", new { Name = model.Name, Category = model.Category, DateCreate = DateTime.Now, Price = model.Price });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Delete(IRepository repository, int id)
        {
            var entity = await repository.DeleteAsync(id);
            return entity is not null ? TypedResults.Ok(entity) : TypedResults.NotFound(new { Error = "Product not found" });
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Update(IRepository repository, int id, ProductPut model)
        {
            var entity = await repository.GetByIdAsync(id);

            if (entity is null) return TypedResults.NotFound(new { Error = "Product not found" });

            if (model.Name != null) entity.Name = model.Name;
            if (model.Category != null) entity.Category = model.Category;
            if (model.Price != null) entity.Price = (int)model.Price;

            var product = await repository.UpdateAsync(id, entity);

            return TypedResults.Created($"https://localhost:7188/products/{entity.Id}", new { Name = entity.Name, Category = entity.Category, DateUpdate = DateTime.Now, Price = entity.Price });
        }
    }
}
