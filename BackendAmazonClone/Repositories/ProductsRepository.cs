using BackendAmazonClone.DTOs;
using BackendAmazonClone.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendAmazonClone.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ApplicationContext _context;
        public ProductsRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<List<Products>> GetAllProducts()
        {
            return await _context.Products.Where(p => p.CartId == null).ToListAsync();
        }

        public async Task<List<Products>> GetAllProductsFromPerson(long findID)
        {
            var cartOfPerson = await _context.Products.Where(p => p.UserId == findID).ToListAsync();

            return cartOfPerson;
        }
        public async Task DeleteProduct(long checkId)
        {
            var itemToRemove = _context.Products.Where(p => p.ProductsId == checkId).FirstOrDefault();

            var CartProductToRemove = await _context.Products.Where(p => p.ParentProductId == checkId).ToListAsync();

            foreach(Products product in CartProductToRemove)
            {
                _context.Products.Remove(product);
            }

            _context.Products.Remove(itemToRemove);
            await _context.SaveChangesAsync();
        }
        public async Task CreateProduct(Products product)
        {
            //_context.Products.Add(product);

            var firstItem =_context.Users.Include(Users => Users.Product).Where(u => u.Id == product.UserId).FirstOrDefault();

            firstItem.Product.Add(product);

            await _context.SaveChangesAsync();

            return;
        }
    }
}
