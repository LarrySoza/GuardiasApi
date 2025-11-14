using App.Core.Entities.Core;
using App.WebApi.Models.Puesto;
using AutoMapper;

namespace App.WebApi.Mapping
{
    public class PuestoProfile : Profile
    {
        public PuestoProfile()
        {
            CreateMap<Puesto, PuestoDto>();
            CreateMap<PuestoDto, Puesto>();
        }
    }
}
