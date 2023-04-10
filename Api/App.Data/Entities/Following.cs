using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Entities
{
    public  class Following : BaseEntity
    {
        public string FollowedId { get; set; }

        [ForeignKey("FollowedId")]
        public AppIdentityUser Followed { get; set; }

        public string FollowerId { get; set; }

        [ForeignKey("FollowerId")]
        public AppIdentityUser Follower { get; set; }
    }
}
