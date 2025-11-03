using App.Core.Entities.Core;
using App.WebApi.Models.Usuario;
using AutoMapper;

namespace App.WebApi.Mapping
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<CrearUsuarioRequestDto, Usuario>();

            CreateMap<Usuario, UsuarioDto>();

            CreateMap<Rol, RolDto>();
        }
    }
}
