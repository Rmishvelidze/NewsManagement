using AutoMapper;
using NewsManagement.Application.Features.Todo.Commands.CreateTodo;
using NewsManagement.Application.Features.Todo.Queries.GetTodo;
using NewsManagement.Application.Features.Todo.Queries.GetTodoList;
using NewsManagement.Domain.Models;

namespace NewsManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Todo, GetTodoListDto>().ForMember
            (
                dest => dest.Status,
                opt => opt.MapFrom
                (
                    src => ((TodoStatus?)src.Status).ToString()
                )
            );

            CreateMap<Todo, GetTodoDto>().ForMember
            (
                dest => dest.Status,
                opt => opt.MapFrom
                (
                    src => ((TodoStatus?)src.Status).ToString()
                )
            );

            CreateMap<CreateTodoCommand, Todo>().ReverseMap();
        }
    }
}