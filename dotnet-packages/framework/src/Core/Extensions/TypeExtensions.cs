using System.Reflection;

namespace Framework.Core.Extensions
{
    public static class TypeExtensions
    {
        public static T Clone<T>(this T item)
        {
            if (item == null) return default;

            return (T)item.GetType().InvokeMember("MemberwiseClone", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic, null, item, null);
        }
    }
}