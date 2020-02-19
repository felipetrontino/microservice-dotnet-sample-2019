using System;

namespace Framework.Core.Entities
{
    public abstract class Entity : BaseEntity, IAuditEntity, IVirtualDeletedEntity, IConcurrencyEntity
    {
        public DateTime InsertedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] RowVersion { get; set; }
    }
}