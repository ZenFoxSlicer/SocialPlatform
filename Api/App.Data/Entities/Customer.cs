using System;
using System.Collections.Generic;
using System.Text;

namespace App.Data.Entities
{
    public class Customer : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public List<Product> Products { get; set; }
    }
}
