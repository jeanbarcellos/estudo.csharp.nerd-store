using System;
using System.Collections.Generic;

namespace NerdStore.Core.DomainObjects.DTO
{
    public class ListaProdutosPedido
    {
        public Guid PedidoId { get; set; }
        public ICollection<Item> Itens { get; set; }

        public ListaProdutosPedido ()
        {

        }

        public ListaProdutosPedido(Guid pedidoId)
        {
            PedidoId = pedidoId;
            Itens = new List<Item>();
        }
    }
}
