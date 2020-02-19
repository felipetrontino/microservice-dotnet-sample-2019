using Framework.Data.EF;

namespace Book.Tools.DbContext
{
    public class DesignTimeDbContextFactory : DesignTimeDbContextFactory<DbMigrations>
    {
        public override string Schema => nameof(Book);
    }
}