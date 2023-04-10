using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Entities
{
    public class Publication : BaseEntity
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Labels { get; set; }
        public DateTime DateTime { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Like> Likes { get; set; }

        public string AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public AppIdentityUser Author { get; set; }
    }
}
