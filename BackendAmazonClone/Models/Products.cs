using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendAmazonClone.Models
{
    public class Products
    {
        public long ProductsId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public long ParentProductId { get; set; }
        public long? CartId { get; set; }
        public long? UserId { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual Users User { get; set; }
    }
}
