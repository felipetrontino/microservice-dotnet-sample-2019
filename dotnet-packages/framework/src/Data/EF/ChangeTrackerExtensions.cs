using Framework.Core.Common;
using Framework.Core.Entities;
using Framework.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Data.EF
{
    public static class ChangeTrackerExtensions
    {
        public static void Configure(this ChangeTracker tracker, DateTime date)
        {
            var entries = tracker.Entries().Where(p => p.State == EntityState.Added
                                                                    || p.State == EntityState.Deleted
                                                                    || p.State == EntityState.Modified);

            foreach (var ent in entries)
            {
                SetAuditInfo(ent, date);
            }
        }

        public static IEnumerable<AuditEntry> GetLogs(this ChangeTracker tracker, string schema, IUserAccessor userAccessor)
        {
            var ret = new List<AuditEntry>();

            var entries = tracker.Entries().Where(p => p.State == EntityState.Added
                                                                   || p.State == EntityState.Deleted
                                                                   || p.State == EntityState.Modified);

            foreach (var ent in entries)
            {
                var auditEntry = new AuditEntry(ent)
                {
                    State = ent.State,
                    UserId = userAccessor.UserName,
                    Schema = schema,
                    TableName = ent.Metadata.GetTableName()
                };

                foreach (var property in ent.Properties)
                {
                    var propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (ent.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:

                            if (property.IsModified)
                            {
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }

                    ret.Add(auditEntry);
                }
            }

            return ret;
        }

        private static void SetAuditInfo(EntityEntry ent, DateTime date)
        {
            var eType = ent.Entity.GetType();
            var isAudit = typeof(IAuditEntity).IsAssignableFrom(eType);
            var isVirtualDeleted = typeof(IVirtualDeletedEntity).IsAssignableFrom(eType);

            if (!isAudit) return;

            switch (ent.State)
            {
                case EntityState.Added:
                    eType.GetProperty(nameof(Entity.InsertedAt))?.SetValue(ent.Entity, date);

                    break;

                case EntityState.Deleted:
                    eType.GetProperty(nameof(Entity.DeletedAt))?.SetValue(ent.Entity, date);

                    break;

                case EntityState.Modified:

                    var isDeleted = Convert.ToBoolean(eType.GetProperty(nameof(Entity.IsDeleted))?.GetValue(ent.Entity));

                    if (isVirtualDeleted && isDeleted)
                        eType.GetProperty(nameof(Entity.DeletedAt))?.SetValue(ent.Entity, date);
                    else
                        eType.GetProperty(nameof(Entity.UpdatedAt))?.SetValue(ent.Entity, date);

                    break;

                case EntityState.Detached:
                case EntityState.Unchanged:
                default:
                    break;
            }
        }
    }
}