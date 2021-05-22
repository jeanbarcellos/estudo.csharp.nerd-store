using System;

namespace NerdStore.Core.DomainObjects.DTO
{
    public class Item
    {
        public Guid Id { get; set; }
        public int Quantidade { get; set; }

        public Item ()
        {

        }

        public Item(Guid id, int quantidade)
        {
            Id = id;
            Quantidade = quantidade;
        }
    }
}
