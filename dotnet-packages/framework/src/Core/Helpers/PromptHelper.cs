using Framework.Core.Common;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Core.Helpers
{
    public static class PromptHelper
    {
        public static T GetOption<T>(string prompt, int? defaultAnswer = null)
            where T : struct
        {
            var type = typeof(T);

            var values = Enum.GetValues(type)
                .Cast<int>()
                .Select<int, (int Id, string Name)>(x => (x, Enum.GetName(type, x)))
                .ToList();

            Console.Write(string.Join(Environment.NewLine, values.Select((v, i) => v.Id + ". " + v.Name).ToArray()) + Environment.NewLine);

            var option = Prompt.GetInt(prompt, defaultAnswer);
            return (T)Enum.Parse(type, values.First(x => x.Id == option).Name);
        }

        [ExcludeFromCodeCoverage]
        public static void Wait()
        {
            if (TestDetector.UnitTestDetector.IsTesting) return;

            while (true)
            {
                Task.Delay(500).Wait();
            }
        }
    }
}