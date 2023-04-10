using App.Data.Entities;
using App.Service.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Dto.Mappings
{
    public class PublicationMappingProfile : Profile
    {
        public PublicationMappingProfile()
        {
            CreateMap();
        }
        public void CreateMap()
        {
            CreateMap<PublicationModel, Publication>().ReverseMap();
            CreateMap<Publication, PublicationExternalModel>().ReverseMap();

            CreateMap<Comment, CommentModel>()
                .ForMember(x => x.AuthorName, opt => opt.MapFrom(y => $"{y.Author.FirstName} {y.Author.LastName}"))
                .ForMember(x => x.AuthorUserName, opt => opt.MapFrom(y => y.Author.UserName));
            CreateMap<Like, LikeModel>()
                .ForMember(x => x.AuthorName, opt => opt.MapFrom(y => $"{y.Author.FirstName} {y.Author.LastName}"));


        }
    }
}
