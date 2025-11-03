using App.Core.Entities.Core;
using App.WebApi.Models.Unidad;
using AutoMapper;

namespace App.WebApi.Mapping
{
    public class UnidadProfile : Profile
    {
        public UnidadProfile()
        {
            CreateMap<CrearUnidadRequestDto, Unidad>();
            CreateMap<ActualizarUnidadRequestDto, Unidad>();
            CreateMap<Unidad, UnidadDto>();
        }
    }
}
