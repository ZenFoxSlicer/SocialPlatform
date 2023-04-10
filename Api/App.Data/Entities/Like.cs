using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Entities
{
    public class Like : BaseEntity
    {
        public Guid PublicationId { get; set; }

        [ForeignKey("PublicationId")]
        public Publication Publication { get; set; }

        public string AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public AppIdentityUser Author { get; set; }
    }
}
