using NerdStore.Core.DomainObjects;
using System.Collections.Generic;

namespace NerdStore.Catalogo.Domain
{
    public class Categoria : Entity
    {
        public string Nome { get; private set; }
        public int Codigo { get; private set; }
        // EF Relation
        public ICollection<Produto> Produtos { get; set; }

        // EF
        protected Categoria () { }

        public Categoria(string nome, int codigo)
        {
            Nome = nome;
            Codigo = codigo;
        }

        public override string ToString()
        {
            return $"{Nome} - {Codigo}";
        }

        public void Validar()
        {
            Validacoes.ValidarSeVazio(Nome, "O campo Nome da Categoria não pode estar vazio.");
            Validacoes.ValidarSeIgual(Codigo, 0, "O campo Código da Categoria não pode ser zero.");
        }
    }
}
