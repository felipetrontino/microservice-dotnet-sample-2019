using Book.Domain.Enums;

namespace Book.Tests.Mocks
{
    public static class Fake
    {
        public static string GetAuthorName(string key)
        {
            return $"{key}_AUTHOR";
        }

        public static string GetCategoryName(string key)
        {
            return $"{key}_CATEGORY";
        }

        public static string GetTitle(string key)
        {
            return $"{key}_TITLE";
        }

        public static Language GetLanguage()
        {
            return Language.English;
        }
    }
}