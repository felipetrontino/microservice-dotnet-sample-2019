using System.Threading.Tasks;

namespace Framework.Core.Bus
{
    public interface IBusContainer : IBusPublisher
    {
        Task CommitAsync();
    }
}