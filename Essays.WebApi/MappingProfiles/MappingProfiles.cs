using AutoMapper;
using Essays.WebApi.DTOs;
using Essays.WebApi.Models;

namespace Essays.WebApi.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Author, AuthorDto>();
            CreateMap<Author, AuthorDto>().ReverseMap();

            CreateMap<Country, CountryDto>();
            CreateMap<Country, CountryDto>().ReverseMap();

            CreateMap<Essay, EssayDto>();
            CreateMap<Essay, EssayDto>().ReverseMap();

            CreateMap<SubjectCategory, SubjectCategoryDto>();
            CreateMap<SubjectCategory, SubjectCategoryDto>().ReverseMap();

            CreateMap<Subject, SubjectDto>();
            CreateMap<Subject, SubjectDto>().ReverseMap();
        }
    }
}