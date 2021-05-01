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
    [Route("api/[controller]")] //api/Cart
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        //api/Cart
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Cart>> UpdateCartObject([FromBody] Products product)
        {
            Products newProduct = new Products
            {
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductPrice = product.ProductPrice,
                ParentProductId = product.ParentProductId,
                CartId = product.CartId
            };

            Cart returnCart = await _cartRepository.UpdateCartProduct(newProduct, product.CartId);

            return Ok(returnCart);
        }

        //api/Cart/5
        [HttpPost("{id}")]
        [Authorize]
        public async Task<ActionResult<Cart>> GetCart(long id, [FromBody] CheckID checkId)
        {
            if(id != checkId.CheckId)
            {
                return Unauthorized();
            }

            List<Cart> getCart = await _cartRepository.GetCart(checkId.CheckId);

            return Ok(getCart);
        }

        //api/Cart
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> RemoveCartProduct([FromBody] CheckID checkId)
        {
            await _cartRepository.RemoveCartProduct(checkId.CheckId);

            return Ok(new { message = "Product removed" });
        }
    }

    public class CheckID
    {
        public long CheckId { get; set; }
    }
}
