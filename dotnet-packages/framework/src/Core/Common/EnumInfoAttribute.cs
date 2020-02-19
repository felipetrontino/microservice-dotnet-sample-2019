namespace Framework.Core.Common
{
    public class EnumInfoAttribute : System.Attribute
    {
        public string[] AlternativeNames { get; }

        public EnumInfoAttribute(params string[] alternativeNames)
        {
            AlternativeNames = alternativeNames;
        }
    }
}