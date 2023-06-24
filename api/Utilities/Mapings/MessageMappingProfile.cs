using AutoMapper;
using ResellHub.DTOs.MessageDTOs;
using ResellHub.Entities;

namespace ResellHub.Utilities.Mapings
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {
            CreateMap<MessageDisplayDto, Message>().ReverseMap();
        }
    }
}
