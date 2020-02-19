using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Framework.Core.Helpers
{
    public static class XmlHelper
    {
        public static string Serialize<T>(T obj)
        {
            if (obj == null) return null;

            using (var ms = new MemoryStream())
            {
                var xmlWriter = new XmlTextWriter(ms, Encoding.Default);
                var serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(xmlWriter, obj, null);

                string ret = null;

                using (var reader = new StreamReader(ms))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    ret = reader.ReadToEnd();
                }

                return ret;
            }
        }

        public static string XmlSerialize<T>(this T obj)
        {
            return Serialize(obj);
        }

        public static T Deserialize<T>(string xml)
        {
            T ret = default;

            if (string.IsNullOrWhiteSpace(xml)) return ret;

            var xs = new XmlSerializer(typeof(T));

            using (TextReader reader = new StringReader(xml))
            {
                ret = (T)xs.Deserialize(reader);
            }

            return ret;
        }

        public static T XmlDeserialize<T>(this string xml)
        {
            return Deserialize<T>(xml);
        }

        public static MemoryStream SerializeInMemory<T>(T obj)
        {
            var dcs = new DataContractSerializer(obj.GetType());

            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                OmitXmlDeclaration = true
            };

            var ms = new MemoryStream();

            using (var writer = XmlWriter.Create(ms, settings))
            {
                dcs.WriteObject(writer, obj);
            }

            return ms;
        }
    }
}