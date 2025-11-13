using App.Application.Models.Turno;
using App.Core.Entities.Core;
using AutoMapper;

namespace App.WebApi.Mapping
{
    public class TurnoProfile : Profile
    {
        public TurnoProfile()
        {
            CreateMap<Turno, TurnoDto>();
            CreateMap<TurnoDto, Turno>();
        }
    }
}
