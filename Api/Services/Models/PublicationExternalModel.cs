using App.Data.Entities;
using System.Collections.Generic;

namespace App.Service.Models
{
    public class PublicationExternalModel : PublicationModel
    {
        public IEnumerable<CommentModel> Comments { get; set; }
        public IEnumerable<LikeModel> Likes { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
    }
}