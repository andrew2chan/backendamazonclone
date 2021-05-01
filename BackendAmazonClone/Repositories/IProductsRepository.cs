using BackendAmazonClone.DTOs;
using BackendAmazonClone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendAmazonClone.Repositories
{
    public interface IProductsRepository
    {
        public Task<List<Products>> GetAllProducts();
        public Task CreateProduct(Products product);
        public Task<List<Products>> GetAllProductsFromPerson(long findID);
        public Task DeleteProduct(long checkId);
    }
}
