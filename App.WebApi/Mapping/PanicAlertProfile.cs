using App.Core.Entities.Core;
using App.WebApi.Models.PanicAlert;
using AutoMapper;

namespace App.WebApi.Mapping
{
    public class PanicAlertProfile : Profile
    {
        public PanicAlertProfile()
        {
            CreateMap<PanicAlert, PanicAlertDto>();
            CreateMap<PanicAlertAdjunto, PanicAlertAdjuntoDto>();

            CreateMap<PanicAlertDto, PanicAlert>();
            CreateMap<PanicAlertAdjuntoDto, PanicAlertAdjunto>();
        }
    }
}
