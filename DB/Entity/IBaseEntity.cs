using System;

namespace DB.Entity
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }
    }
}
