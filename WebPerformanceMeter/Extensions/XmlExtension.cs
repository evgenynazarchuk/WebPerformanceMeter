using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace WebPerformanceMeter.Extensions
{
    public static class XmlExtension
    {
        public static string FromObjectToXmlString<T>(
            this T value,
            XmlWriterSettings xmlSettings)
            where T : class
        {
            XmlSerializerNamespaces? namespaces = new(new[] { XmlQualifiedName.Empty });

            using StringWriter stringWriter = new();
            using XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlSettings);
            XmlSerializer serializer = new(typeof(T));
            serializer.Serialize(xmlWriter, value, namespaces);

            return stringWriter.ToString();
        }

        public static T? FromXmlStringToObject<T>(this string value)
            where T : class
        {
            using var reader = new StringReader(value);
            var serializer = new XmlSerializer(typeof(T));
            return serializer.Deserialize(reader) as T;
        }
    }
}
