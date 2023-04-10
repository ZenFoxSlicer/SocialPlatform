using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Entities
{
    public class LikeModel : BaseEntity
    {
        public Guid PublicationId { get; set; }
        public string AuthorName { get; set; }
    }
}
