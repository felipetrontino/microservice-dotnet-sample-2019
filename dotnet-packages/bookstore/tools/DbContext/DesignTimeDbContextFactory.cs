using Framework.Data.EF;

namespace Bookstore.Tools.DbContext
{
    public class DesignTimeDbContextFactory : DesignTimeDbContextFactory<DbMigrations>
    {
        public override string Schema => nameof(Bookstore);
    }
}