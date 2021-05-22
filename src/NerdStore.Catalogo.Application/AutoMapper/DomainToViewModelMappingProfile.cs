using AutoMapper;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Catalogo.Domain;

namespace NerdStore.Catalogo.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Categoria, CategoriaViewModel>();

            CreateMap<Produto, ProdutoViewModel>()
                .ForMember(d => d.Altura, opts => opts.MapFrom(o => o.Dimensoes.Altura))
                .ForMember(d => d.Largura, opts => opts.MapFrom(o => o.Dimensoes.Largura))
                .ForMember(d => d.Profundidade, opts => opts.MapFrom(o => o.Dimensoes.Profundidade));
        }
    }
}
