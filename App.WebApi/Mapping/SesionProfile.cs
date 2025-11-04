using App.Core.Entities.Core;
using App.WebApi.Models.Sesion;
using AutoMapper;

namespace App.WebApi.Mapping
{
    public class SesionProfile : Profile
    {
        public SesionProfile()
        {
            CreateMap<SesionUsuario, SesionUsuarioDto>();
        }
    }
}