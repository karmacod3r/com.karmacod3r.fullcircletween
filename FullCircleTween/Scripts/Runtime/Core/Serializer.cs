using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace FullCircleTween.Core
{
    public static class Serializer
    {
        public static string SerializeObject<T>(T serializableObject)
        {
            var xmlSerializer = new XmlSerializer(serializableObject.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, serializableObject);
                return textWriter.ToString();
            }
        }

        public static T DeserializeObject<T>(string serializedObject)
        {
            try
            {
                var x = new XmlSerializer(typeof(T));
                using (var stream = new StringReader(serializedObject))
                {
                    return (T)x.Deserialize(stream);
                }
            } catch (Exception)
            {
                return Activator.CreateInstance<T>();
            }
        }

        public static object DeserializeObject(Type type, string serializedObject)
        {
            if (type == null) return null;

            try
            {
                var x = new XmlSerializer(type);
                using (var stream = new StringReader(serializedObject))
                {
                    return x.Deserialize(stream);
                }
            } catch (Exception)
            {
                return type.IsValueType ? Activator.CreateInstance(type) : null;
            }
        }
    }
}