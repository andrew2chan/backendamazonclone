using BackendAmazonClone.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BackendAmazonClone.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationContext _context;
        public CartRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<Cart> UpdateCartProduct(Products newProduct, long? cartId)
        {
            //_context.Products.Add(newProduct);
            var firstItem = _context.Cart.Include(Products => Products.CartProduct).Where(c => c.CartId == cartId).FirstOrDefault();

            firstItem.CartProduct.Add(newProduct);

            await _context.SaveChangesAsync();

            return firstItem;
        }
        public async Task<List<Cart>> GetCart(long checkId)
        {
            return await _context.Cart.Include(Products => Products.CartProduct).Where(c => c.UserId == checkId).ToListAsync();
        }

        public async Task RemoveCartProduct(long productIdToRemove)
        {
            var productToRemove = await _context.Products.FindAsync(productIdToRemove);
            _context.Remove(productToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
