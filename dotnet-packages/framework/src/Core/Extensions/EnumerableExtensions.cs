using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static Task ForEachAsync<T>(this IEnumerable<T> source, int dop, Func<T, Task> body)
        {
            return Task.WhenAll(Partitioner.Create(source)
                .GetPartitions(dop)
                .Select(partition => Task.Run(async () =>
                {
                    using (partition)
                    {
                        while (partition.MoveNext())
                        {
                            await body(partition.Current);
                        }
                    }
                })));
        }
    }
}