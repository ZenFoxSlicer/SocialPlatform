using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Entities
{
    public class CommentModel 
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public DateTime DateTime { get; set; }

        public Guid PublicationId { get; set; }

        public string AuthorName { get; set; }

        public string AuthorUserName { get; set; }

    }
}
