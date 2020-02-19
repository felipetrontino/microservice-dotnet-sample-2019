using Framework.Core.Common;

namespace Framework.Test.Mock.Common
{
    public class UserAccessorStub : IUserAccessor
    {
        public static string GetUserName(string key = null) => $"{key}_USER";

        public string UserName => GetUserName();

        public static IUserAccessor Create() => new UserAccessorStub();
    }
}