using AutoMapper;
using ResellHub.DTOs.ChatDTOs;
using ResellHub.Entities;

namespace ResellHub.Utilities.Mapings
{
    public class ChatMappingProfile : Profile
    {
        public ChatMappingProfile()
        {
            CreateMap<ChatDisplayDto, Chat>().ReverseMap();
        }
    }
}
