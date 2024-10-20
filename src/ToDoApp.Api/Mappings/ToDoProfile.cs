using AutoMapper;
using ToDoApp.Api.DTOs;
using ToDoApp.Core.Entities;

namespace ToDoApp.Api.Mappings;

public class ToDoProfile : Profile
{
    public ToDoProfile()
    {
        CreateMap<CreateToDoDto, ToDo>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Complete, opt => opt.MapFrom(src => 0));
        
        CreateMap<ToDo, ToDoDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}