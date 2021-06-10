using DB.Entity;
using System;
using System.Collections.Generic;

namespace RefactorThis.DB.Entity
{
    public class Product: IBaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DeliveryPrice { get; set; }
        public List<ProductOption> ProductOptions { get; set; }
    }
}
