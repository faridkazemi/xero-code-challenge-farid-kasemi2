using RefactorThis.DB.Entity;
using RefactorThis.Mapper.Interface;
using RefactorThis.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RefactorThis.Mapper
{
    public class ProductOptionMapper : IProductOptionMapper
    {
        public ProductOptionDto ToDto(ProductOption entity)
        {
            return new ProductOptionDto
            {
                Id = entity.Id,
                Description = entity.Description,
                Name = entity.Name
            };
        }

        public IEnumerable<ProductOptionDto> ToDto(IEnumerable<ProductOption> entities)
        {
            return entities.Select(e => new ProductOptionDto
            {
                Id = e.Id,
                Description = e.Description,
                Name = e.Name
            });
        }

        public ProductOption ToEntity(ProductOptionDto dto, Guid productId)
        {
            return new ProductOption
            {
                Id = productId == Guid.Empty ? Guid.NewGuid() : dto.Id,
                Description = dto.Description,
                Name = dto.Name,
                ProductId = productId
            };
        }

        public ProductOption ToEntity(ProductOptionDto dto, Guid id, Guid productId)
        {
            return new ProductOption
            {
                Id = id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                Description = dto.Description,
                Name = dto.Name,
                ProductId = productId
            };
        }

        public ProductOptionListDto ToDtoList(IEnumerable<ProductOption> entities)
        {
            return new ProductOptionListDto
            {
                Items = entities.Select(e => new ProductOptionDto 
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description
                }).ToList()
            };
        }

        // There is no case to use this at the moment
        public IEnumerable<ProductOption> ToEntity(IEnumerable<ProductOptionDto> dtos)
        {
            throw new NotImplementedException();
        }
    }
}
