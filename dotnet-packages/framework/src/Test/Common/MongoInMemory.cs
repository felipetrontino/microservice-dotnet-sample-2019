using Mongo2Go;
using MongoDB.Driver;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace Framework.Test.Common
{
    [ExcludeFromCodeCoverage]
    public static class MongoInMemory
    {
        private static readonly object Lock = new object();
        public static readonly string MongoTestPath = Path.Combine(@"C:\mongo2go\");
        private static MongoDbRunner _runner;

        public static string GetConnectionString(string key)
        {
            lock (Lock)
            {
                if (!Directory.Exists(MongoTestPath))
                    Directory.CreateDirectory(MongoTestPath);

                if (_runner == null)
                {
                    _runner = MongoDbRunner.Start(MongoTestPath);
                }

                var builder = new MongoUrlBuilder(_runner.ConnectionString)
                {
                    DatabaseName = key
                };

                return builder.ToString();
            }
        }

        public static void DisposeMongoDbRunner()
        {
            var processes = Process.GetProcessesByName("mongod");
            foreach (var process in processes)
            {
                if (process.MainModule.FileName.Contains("mongo2go"))
                {
                    process.Kill();
                }
            }

            TryDeleteFolder();
        }

        private static void TryDeleteFolder()
        {
            var delete = false;
            while (!delete)
            {
                try
                {
                    if (Directory.Exists(MongoTestPath))
                        Directory.Delete(MongoTestPath, true);

                    delete = true;
                }
                catch (IOException)
                {
                    delete = false;
                    Task.Delay(250).Wait();
                }
            }
        }
    }
}