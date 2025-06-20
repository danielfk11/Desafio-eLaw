using AutoMapper;
using ClienteApi.DTOs;
using ClienteApi.Models;

namespace ClienteApi.Extensions
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<ClienteCreateDto, Cliente>();

            CreateMap<Endereco, EnderecoDto>().ReverseMap();
        }
    }
}
