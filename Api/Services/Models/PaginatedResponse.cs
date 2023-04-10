using System.Collections.Generic;

namespace App.Service.Models
{
    public class PaginatedResponse<T>
    {
        public int TotalData { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
