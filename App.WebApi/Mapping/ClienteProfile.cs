using App.Core.Entities.Core;
using App.WebApi.Models.Cliente;
using AutoMapper;

namespace App.WebApi.Mapping
{
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            CreateMap<CrearClienteRequestDto, Cliente>();
            CreateMap<ActualizarClienteRequestDto, Cliente>();
            CreateMap<Cliente, ClienteDto>();
        }
    }
}
