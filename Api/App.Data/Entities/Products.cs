using System;
using System.Collections.Generic;
using System.Text;

namespace App.Data.Entities
{
    public class Products : BaseEntity
    {
        public long CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string VideoLink { get; set; }
        public string Picture { get; set; } 
    }
}
