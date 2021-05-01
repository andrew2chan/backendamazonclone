using BackendAmazonClone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendAmazonClone.Repositories
{
    public interface ICartRepository
    {
        public Task<Cart> UpdateCartProduct(Products newProduct, long? cartId);
        public Task<List<Cart>> GetCart(long checkId);
        public Task RemoveCartProduct(long productIdToRemove);
    }
}
