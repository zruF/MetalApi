using AutoMapper;
using MetalModels;
using MetalModels.Models;
using MetalModels.Types;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.Responses;

namespace MetalServices.Mapping
{
    public class MetalProfile : Profile
    {
        public MetalProfile()
        {
            CreateMap<Band, BandResponse>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.BandGenres.Select(b => b.Genre.Name)))
                .ForMember(dest => dest.Albums, opt => opt.MapFrom(src => src.Albums));

            CreateMap<Album, BandAlbumResponse>();

            CreateMap<Album, AlbumResponse>();

            CreateMap<User, UserResponse>();

            CreateMap<Band, SearchResponse>()
                .ForMember(dest => dest.EntityType, opt => opt.MapFrom(src => EntityType.Band.ToString()));

            CreateMap<Album, SearchResponse>()
                .ForMember(dest => dest.EntityType, opt => opt.MapFrom(src => EntityType.Album.ToString()));

            CreateMap<Genre, SearchResponse>()
                .ForMember(dest => dest.EntityType, opt => opt.MapFrom(src => EntityType.Genre.ToString()));
        }
    }
}
