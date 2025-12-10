using AutoMapper;
using Million.Properties.Application.DTOs;
using Million.Properties.Domain.Entities;
using Million.Properties.Domain.Entities.Request.Properties;

namespace Million.Properties.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreatePropertyRequest, Property>()
            .ForMember(dest => dest.IdProperty, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.CodeInternal, opt => opt.MapFrom(src => 
                src.InternalCode.HasValue && src.InternalCode.Value != Guid.Empty 
                    ? src.InternalCode.Value.ToString() 
                    : Guid.NewGuid().ToString()))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
            .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.IdOwner))
            .ForMember(dest => dest.Owner, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.Traces, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore());

        CreateMap<CreatePropertyDto, Property>()
            .ForMember(dest => dest.IdProperty, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.CodeInternal, opt => opt.MapFrom(src => src.InternalCode))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
            .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.IdOwner))
            .ForMember(dest => dest.Owner, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.Traces, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore());

        CreateMap<PropertyImage, PropertyImageDto>()
            .ForMember(d => d.IdPropertyImage, o => o.MapFrom(s => s.IdPropertyImage))
            .ForMember(d => d.File, o => o.MapFrom(s => s.File))
            .ForMember(d => d.Enabled, o => o.MapFrom(s => s.Enabled))
            .ForMember(d => d.IdProperty, o => o.MapFrom(s => s.IdProperty));

        CreateMap<Property, PropertyDto>()
            .ForMember(d => d.IdProperty, o => o.MapFrom(s => s.IdProperty))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
            .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
            .ForMember(d => d.Price, o => o.MapFrom(s => s.Price))
            .ForMember(d => d.InternalCode, o => o.MapFrom(s => s.CodeInternal))
            .ForMember(d => d.Year, o => o.MapFrom(s => s.Year))
            .ForMember(d => d.IdOwner, o => o.MapFrom(s => s.IdOwner))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

        CreateMap<PropertyDto, Property>()
            .ForMember(d => d.IdProperty, o => o.MapFrom(s => s.IdProperty))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
            .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
            .ForMember(d => d.Price, o => o.MapFrom(s => s.Price))
            .ForMember(d => d.CodeInternal, o => o.MapFrom(s => s.InternalCode))
            .ForMember(d => d.Year, o => o.MapFrom(s => s.Year))
            .ForMember(d => d.IdOwner, o => o.MapFrom(s => s.IdOwner))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
            .ForMember(dest => dest.Owner, opt => opt.Ignore())
            .ForMember(dest => dest.Traces, opt => opt.Ignore())
            .ForMember(d => d.CreatedOn, o => o.Ignore())
            .ForMember(d => d.UpdatedOn, o => o.Ignore());

        CreateMap<Property, CreatePropertyDto>()
            .ForMember(d => d._id, o => o.MapFrom(s => s.IdProperty.ToString()))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
            .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
            .ForMember(d => d.Price, o => o.MapFrom(s => s.Price))
            .ForMember(d => d.InternalCode, o => o.MapFrom(s => s.CodeInternal))
            .ForMember(d => d.Year, o => o.MapFrom(s => s.Year))
            .ForMember(d => d.IdOwner, o => o.MapFrom(s => s.IdOwner))
            .ForMember(d => d.File, o => o.Ignore());

        CreateMap<Property, UpdatePropertyDto>()
            .ForMember(d => d.IdProperty, o => o.MapFrom(s => s.IdProperty))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
            .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
            .ForMember(d => d.Price, o => o.MapFrom(s => s.Price))
            .ForMember(d => d.InternalCode, o => o.MapFrom(s => Guid.Parse(s.CodeInternal)))
            .ForMember(d => d.Year, o => o.MapFrom(s => s.Year))
            .ForMember(d => d.IdOwner, o => o.MapFrom(s => s.IdOwner.HasValue ? s.IdOwner.Value.ToString() : null))
            .ForMember(d => d.File, o => o.Ignore());

        CreateMap<UpdatePropertyRequest, Property>()
            .ForMember(dest => dest.IdProperty, opt => opt.MapFrom(src => src.IdProperty))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.CodeInternal, opt => opt.MapFrom(src => src.InternalCode.ToString()))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
            .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.IdOwner))
            .ForMember(dest => dest.Owner, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.Traces, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore());
    }
}
