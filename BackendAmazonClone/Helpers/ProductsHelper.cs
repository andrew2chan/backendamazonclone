using BackendAmazonClone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BackendAmazonClone.Helpers
{
    public class ProductsHelper
    {
        public ProductsHelper()
        {
        }

        //@return true = error
        public bool CheckNewProductFieldsIsEmpty(Products product)
        {
            if (String.IsNullOrEmpty(product.ProductName) || String.IsNullOrEmpty(product.ProductDescription) || String.IsNullOrEmpty(product.ProductPrice.ToString()) || product.ProductPrice == 0)
            {
                return true;
            }
            return false;
        }

        public bool CheckNewProductPrice(Products product)
        {
            string price = product.ProductPrice.ToString();
            Regex rx = new Regex(@"^\w*(\.\w{0,2})?$");

            MatchCollection mc = rx.Matches(price);

            if(mc.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
