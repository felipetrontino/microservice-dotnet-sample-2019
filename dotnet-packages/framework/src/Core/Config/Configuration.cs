using Framework.Core.Common;
using Framework.Core.Extensions;
using Framework.Core.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Framework.Core.Config
{
    public static class Configuration
    {
        public static readonly ValueGetter<EnvironmentType> Stage = new ValueGetter<EnvironmentType>(
            () => EnumHelper.ParseTo<EnvironmentType>(Environment.GetEnvironmentVariable(EnvironmentVariables.Environment)));

        public static readonly ValueGetter<string> AppName = new ValueGetter<string>(
            () => Environment.GetEnvironmentVariable(EnvironmentVariables.Application)?.ToPascalCase() ?? "APP");

        public static readonly ValueGetter<bool> Debugging = new ValueGetter<bool>(
            () => Environment.GetEnvironmentVariable(EnvironmentVariables.Debug).ToBoolean());

        public static readonly ValueGetter<bool> DebuggingSql = new ValueGetter<bool>(
           () => Environment.GetEnvironmentVariable(EnvironmentVariables.DebugSql).ToBoolean());

        public static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder().PrepareBuilder().Build();
        }

        public static IConfigurationBuilder PrepareBuilder(this IConfigurationBuilder config)
        {
            config.SetBasePath(Directory.GetCurrentDirectory());

            config.AddJsonFile("appsettings.json", true, true)
                  .AddJsonFile($"appsettings.{Stage.Get().GetName().ToLower()}.json", true, true);

            config.AddEnvironmentVariables();

            return config;
        }

        public static class SectionNames
        {
            public const string App = "AppSettings";
            public const string Feature = "FeatureSettings";
        }

        public static class EnvironmentVariables
        {
            public const string Environment = "ASPNETCORE_ENVIRONMENT";
            public const string Application = "ASPNETCORE_APPLICATION";
            public const string Debug = "APP_DEBUGGING";
            public const string DebugSql = "APP_DEBUGGING_SQL";
        }
    }
}