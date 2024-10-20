using AutoMapper;
using ToDoApp.Api.DTOs;
using ToDoApp.Core.Entities;

namespace ToDoApp.Api.Mappings;

public class ToDoProfile : Profile
{
    public ToDoProfile()
    {
        CreateMap<CreateToDoDto, ToDo>()
            .ForMember(dest => dest.Complete, opt => opt.MapFrom(src => 0));
        
        CreateMap<ToDo, ToDoDto>().ReverseMap();
    }
}