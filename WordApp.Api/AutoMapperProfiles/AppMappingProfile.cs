using AutoMapper;
using WordApp.Persistence.Models;
using WordApp.Shared.Dtos.AuthDtos;
using WordApp.Shared.Dtos.UserWordsDtos;
using WordApp.Shared.Dtos.WordSetDtos;

namespace WordApp.Api.AutoMapperProfiles
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<RefreshToken, RefreshTokenDto>();
            CreateMap<UserWord, UserWordDto>();
            CreateMap<Word, WordDto>();
            CreateMap<WordSet, WordSetDto>();
            CreateMap<Complaint, ComplaintDto>()
                .ForMember(d => d.UserId, d => d.MapFrom(x => x.User.Id))
                .ForMember(d => d.Reason, d => d.MapFrom(x => $"{x.Reason.Name}: {x.Reason.Description}"));
            CreateMap<ProposedWord, ProposedWordDto>();
            CreateMap<UserDto, UserInfo>();
        }
    }
}
