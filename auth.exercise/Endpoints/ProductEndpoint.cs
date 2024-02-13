using auth.exercise.Repository;
using auth.exercise.Model;
using Microsoft.AspNetCore.Mvc;
using auth.exercise.Enums;
using Microsoft.AspNetCore.Authorization;

namespace auth.exercise.Endpoints
{
    public static class ProductEndpoint
    {
        public static void ConfigureProductEndpoint(this WebApplication app)
        {
            var store = app.MapGroup("/products");
            store.MapGet("/", GetAllProducts);
            store.MapPost("/", CreateProduct);
            store.MapPut("/{id}", UpdateProduct);
            store.MapDelete("/{id}", DeleteProduct);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        //Authorize only for Admin
        [Authorize(Roles = nameof(Roles.Admin))]
        public static async Task<IResult> GetAllProducts(IProductRepository productRepository)
        {
            var products = await productRepository.GetAllProducts();
            return TypedResults.Ok(products);
        }

        public static async Task<IResult> CreateProduct(IProductRepository productRepository, ProductPostPayload postPayload)
        {
            var product = await productRepository.AddProduct(postPayload);
            return TypedResults.Ok(product);
        }

        public static async Task<IResult> UpdateProduct(IProductRepository productRepository, int id, ProductPutPayload productPutPayload)
        {
            var product = await productRepository.UpdateProduct(productPutPayload, id);
            if (product == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(product);
        }

        public static async Task<IResult> DeleteProduct(IProductRepository productRepository, int id)
        {
            var product = await productRepository.DeleteProduct(id);
            if (product == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(product);
        }
    }
}