namespace App.Service.Models
{
    public class PaginatedRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
    }
}
