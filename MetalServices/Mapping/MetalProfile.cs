using AutoMapper;
using MetalModels.Models;
using Shared.Dtos.Responses;

namespace MetalServices.Mapping
{
    public class MetalProfile : Profile
    {
        public MetalProfile()
        {
            CreateMap<Band, BandResponse>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.BandGenres.Select(b => b.Genre.Name)));
            CreateMap<Album, AlbumResponse>();
            CreateMap<Song, SongResponse>();
        }
    }
}
