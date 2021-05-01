using BackendAmazonClone.DTOs;
using BackendAmazonClone.Helpers;
using BackendAmazonClone.Models;
using BackendAmazonClone.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendAmazonClone.Controllers
{
    [Route("api/[controller]")] //api/Products
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepository _productsRepository;
        private ProductsHelper _productsHelper;
        public ProductsController(IProductsRepository productRepository)
        {
            _productsRepository = productRepository;
            _productsHelper = new ProductsHelper();
        }

        [HttpGet]
        public async Task<List<Products>> GetAllProducts()
        {
            return await _productsRepository.GetAllProducts();
        }

        [HttpPost("{id}")]
        [Authorize]
        //api/Products/5
        public async Task<ActionResult<List<Products>>> GetAllProductsFromPerson(long id, [FromBody] CheckID checkId)
        {
            if (id != checkId.CheckId)
            {
                return Unauthorized();
            }

            var returnProducts = await _productsRepository.GetAllProductsFromPerson(id);

            return returnProducts;
        }

        [HttpDelete("{id}")]
        [Authorize]
        //api/Products/5
        public async Task<ActionResult> DeleteProduct(long id, [FromBody] RemoveProductType checkId)
        {
            if (id != checkId.CheckId)
            {
                return Unauthorized();
            }

            await _productsRepository.DeleteProduct(checkId.ProductToRemove);

            return Ok(new { message = "Product is removed" });
        }

        [HttpPost]
        [Authorize]
        //api/Products
        public async Task<ActionResult> CreateProduct(Products product)
        {
            bool checkFields = _productsHelper.CheckNewProductFieldsIsEmpty(product);
            bool checkPrice = _productsHelper.CheckNewProductPrice(product);

            if (checkFields == true)
            {
                return BadRequest(new { message = "Please fill in all fields" });
            }
            if (checkPrice == true)
            {
                return BadRequest(new { message = "Please enter a valid price. IE. 3.22" });
            }

            Products newProduct = new Products()
            {
                ProductsId = product.ProductsId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductPrice = product.ProductPrice,
                UserId = product.UserId,
            };

            await _productsRepository.CreateProduct(newProduct);

            return Ok(new { message = "Product created" });
        }
    }

    public class RemoveProductType
    {
        public long CheckId { get; set; }
        public long ProductToRemove { get; set; }
    }
}
