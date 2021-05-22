using AutoMapper;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Catalogo.Domain;
using NerdStore.Catalogo.Domain.Services;
using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Application.Services
{
    public class ProdutoAppService : IProdutoAppService
    {
        private readonly IMapper _mapper;
        private readonly IEstoqueService _estoqueService;
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoAppService(
            IMapper mapper,
            IEstoqueService estoqueService,
            IProdutoRepository produtoRepository
        )
        {
            _mapper = mapper;
            _estoqueService = estoqueService;
            _produtoRepository = produtoRepository;
        }

        public async Task<IEnumerable<ProdutoViewModel>> ObterPorCategoria(int codigo)
        {
            return _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterPorCategoria(codigo));
        }

        public async Task<ProdutoViewModel> ObterPorId(Guid id)
        {
            return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
        }

        public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterTodos());
        }

        public async Task<IEnumerable<CategoriaViewModel>> ObterCategorias()
        {
            return _mapper.Map<IEnumerable<CategoriaViewModel>>(await _produtoRepository.ObterCategorias());
        }

        public async Task AdicionarProduto(ProdutoViewModel produtoViewModel)
        {
            var produto = _mapper.Map<Produto>(produtoViewModel);

            _produtoRepository.Adicionar(produto);

            await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task AtualizarProduto(ProdutoViewModel produtoViewModel)
        {
            var produto = _mapper.Map<Produto>(produtoViewModel);

            _produtoRepository.Atualizar(produto);

            await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<ProdutoViewModel> DebitarEstoque(Guid id, int quantidade)
        {
            // if (!await _estoqueService.DebitarEstoque(id, quantidade)) //Forma 1
            if (!_estoqueService.DebitarEstoque(id, quantidade).Result) // Forma 2
            {
                throw new DomainException("Falha ao debitar estoque.");
            }

            var produto = await _produtoRepository.ObterPorId(id);

            return _mapper.Map<ProdutoViewModel>(produto);
        }

        public async Task<ProdutoViewModel> ReporEstoque(Guid id, int quantidade)
        {
            if (!_estoqueService.ReporEstoque(id, quantidade).Result)
            {
                throw new DomainException("Falha ao repor estoque.");
            }

            var produto = await _produtoRepository.ObterPorId(id);

            return _mapper.Map<ProdutoViewModel>(produto);
        }

        public void Dispose()
        {
            _estoqueService?.Dispose();
            _produtoRepository?.Dispose();
        }

    }
}
