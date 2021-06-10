using System;
using System.Collections.Generic;

namespace RefactorThis.Mapper.Interface
{
    public interface IGenericMapper<TEntity, TDto, TDtolist>
    {
        TDto ToDto(TEntity entity);
        IEnumerable<TDto> ToDto(IEnumerable<TEntity> entities);
        TEntity ToEntity(TDto dto, Guid id);
        IEnumerable<TEntity> ToEntity(IEnumerable<TDto> dtos);

        TDtolist ToDtoList(IEnumerable<TEntity> entities);

    }
}
