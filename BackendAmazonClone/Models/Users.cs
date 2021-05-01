using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendAmazonClone.Models
{
    public class Users
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Address { get; set; }
        public String Password { get; set; }
        public byte[] Salt { get; set; }
        public String HashedPassword { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual List<Products> Product { get; set; }
    }
}
