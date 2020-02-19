using Framework.Data.EF;

namespace Library.Tools.DbContext
{
    public class DesignTimeDbContextFactory : DesignTimeDbContextFactory<DbMigrations>
    {
        public override string Schema => nameof(Library);
    }
}