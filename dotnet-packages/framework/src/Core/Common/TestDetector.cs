using System;
using System.Linq;

namespace Framework.Core.Common
{
    public static class TestDetector
    {
        public static class UnitTestDetector
        {
            static UnitTestDetector()
            {
                const string testAssemblyName = "xunit.runner";

                IsTesting = AppDomain.CurrentDomain.GetAssemblies()
                    .Any(a => a.FullName.StartsWith(testAssemblyName));
            }

            public static bool IsTesting { get; private set; }
        }
    }
}