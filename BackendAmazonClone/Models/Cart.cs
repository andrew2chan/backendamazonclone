using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendAmazonClone.Models
{
    public class Cart
    {
        public long CartId { get; set; }
        public long UserId { get; set; }
        public virtual Users User { get; set; }
        public virtual List<Products> CartProduct { get; set; }
    }
}
