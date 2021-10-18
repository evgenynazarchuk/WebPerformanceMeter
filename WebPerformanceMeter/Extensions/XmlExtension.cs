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
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(typeof(T));
            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, xmlSettings);
            
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
