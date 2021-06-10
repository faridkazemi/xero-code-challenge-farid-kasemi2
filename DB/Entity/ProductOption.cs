using DB.Entity;
using System;

namespace RefactorThis.DB.Entity
{
    public class ProductOption: IBaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
