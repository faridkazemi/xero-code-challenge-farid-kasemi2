using RefactorThis.DB.Entity;
using RefactorThis.Models;
using System;

namespace RefactorThis.Mapper.Interface
{
    public interface IProductOptionMapper : IGenericMapper<ProductOption, ProductOptionDto, ProductOptionListDto>
    {
        ProductOption ToEntity(ProductOptionDto dto, Guid id, Guid productId);
    }
}
