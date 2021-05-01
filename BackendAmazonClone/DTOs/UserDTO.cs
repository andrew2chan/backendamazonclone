using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendAmazonClone.DTOs
{
    public class UserDTO
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Address { get; set; }
    }
}
