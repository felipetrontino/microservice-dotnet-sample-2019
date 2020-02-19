using System;
using System.Diagnostics.CodeAnalysis;

namespace Framework.Test.Common
{
    [ExcludeFromCodeCoverage]
    public class DatabaseFixture : IDisposable
    {
        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            MongoInMemory.DisposeMongoDbRunner();
        }

        ~DatabaseFixture()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}