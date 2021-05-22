using AutoMapper;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Catalogo.Domain;

namespace NerdStore.Catalogo.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<CategoriaViewModel, Categoria>()
                .ConstructUsing(c => new Categoria(c.Nome, c.Codigo));

            CreateMap<ProdutoViewModel, Produto>()
                .ConstructUsing(c =>
                    new Produto(
                        c.Nome, c.Descricao, c.Ativo, c.Valor, c.CategoriaId, c.DataCadastro, c.Imagem,
                        new Dimensoes(c.Altura, c.Largura, c.Profundidade)
                    )
                );
        }
    }
}