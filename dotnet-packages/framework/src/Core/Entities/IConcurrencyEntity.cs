namespace Framework.Core.Entities
{
    public interface IConcurrencyEntity
    {
        byte[] RowVersion { get; set; }
    }
}