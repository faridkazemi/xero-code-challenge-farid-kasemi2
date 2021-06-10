using RefactorThis.DB.Entity;
using RefactorThis.Mapper.Interface;
using RefactorThis.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RefactorThis.Mapper
{
    public class ProductMapper : IProductMapper
    {
        public ProductDto ToDto(Product entity)
        {
            return new ProductDto
            {
                Id = entity.Id,
                DeliveryPrice = entity.DeliveryPrice,
                Description = entity.Description,
                Name = entity.Name,
                Price = entity.Price
            };
        }

        public IEnumerable<ProductDto> ToDto(IEnumerable<Product> entites)
        {
            return entites.Select(e => new ProductDto
            {
                Id = e.Id,
                DeliveryPrice = e.DeliveryPrice,
                Description = e.Description,
                Name = e.Name,
                Price = e.Price
            });
        }

        public Product ToEntity(ProductDto dto, Guid id)
        {
            return new Product
            {
                Id = id == Guid.Empty ? Guid.NewGuid() : id,
                DeliveryPrice = dto.DeliveryPrice,
                Description = dto.Description,
                Name = dto.Name,
                Price = dto.Price
            };
        }

        // There is no case to use this at the moment
        public IEnumerable<Product> ToEntity(IEnumerable<ProductDto> dtos)
        {
            throw new NotImplementedException();
        }

        public ProductListDto ToDtoList(IEnumerable<Product> entities)
        {
            return new ProductListDto
            {
                Items = entities.Select(e => new ProductDto
                {
                    Id = e.Id,
                    DeliveryPrice = e.DeliveryPrice,
                    Description = e.Description,
                    Name = e.Name,
                    Price = e.Price
                }).ToList()
            };
        }
    }
}
