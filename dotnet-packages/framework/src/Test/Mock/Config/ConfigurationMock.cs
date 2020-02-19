using Framework.Core.Extensions;
using System.Collections.Generic;

namespace Framework.Test.Mock.Config
{
    public class ConfigurationMock
    {
        private readonly Dictionary<string, string> _dic = new Dictionary<string, string>();

        public static ConfigurationMock Create() => new ConfigurationMock();

        public ConfigurationMock Add<T>(T settings, params string[] sectionNames)
        {
            if (settings == null) return this;

            var dic = settings.ToDictionaryConfig(sectionNames);

            foreach (var key in dic.Keys)
            {
                _dic.Add(key, dic[key]);
            }

            return this;
        }

        public Dictionary<string, string> Build()
        {
            return _dic;
        }
    }
}