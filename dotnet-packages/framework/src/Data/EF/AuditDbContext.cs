using Framework.Core.Common;
using Framework.Data.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Framework.Data.EF
{
    public class AuditDbContext : EfDbContext
    {
        private IEnumerable<AuditEntry> _auditLogs = new List<AuditEntry>();

        public AuditDbContext(DbContextOptions options, IUserAccessor userAccessor)
            : base(options)
        {
            UserAccessor = userAccessor;
        }

        protected IUserAccessor UserAccessor { get; }

        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnBeforeSaveChanges()
        {
            var date = DateTime.UtcNow;

            this.ChangeTracker.Configure(date);

            _auditLogs = this.ChangeTracker.GetLogs(Schema, UserAccessor);
        }

        protected override void OnAfterSaveChanges()
        {
            foreach (var log in _auditLogs)
            {
                this.AuditLogs.AddAsync(log.ToAudit());
            }

            base.SaveChangesAsync();
        }
    }
}