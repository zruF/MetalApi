﻿using AutoMapper;
using MetalModels.Models;
using MetalModels.Types;
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

            CreateMap<Band, SearchResponse>()
                .ForMember(dest => dest.EntityType, opt => opt.MapFrom(src => EntityType.Band.ToString()));

            CreateMap<Album, SearchResponse>()
                .ForMember(dest => dest.EntityType, opt => opt.MapFrom(src => EntityType.Album.ToString()));

            CreateMap<Song, SearchResponse>()
                .ForMember(dest => dest.EntityType, opt => opt.MapFrom(src => EntityType.Song.ToString()));
        }
    }
}
