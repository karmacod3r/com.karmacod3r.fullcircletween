using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace FullCircleTween.Extensions
{
    /// <summary>
    /// Extensions for the SerializedProperty class 
    /// </summary>
    public static class SerializedPropertyExtension
    {
        /// <summary>
        /// Parses the serialized property's path to get a reference to the declaring object via reflection 
        /// </summary>
        /// <param name="prop">Current SerializedProperty instance</param>
        /// <returns></returns>
        public static object GetTarget(this SerializedProperty prop)
        {
            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetArrayValue(obj, elementName, index);
                } else
                {
                    obj = GetValue(obj, element);
                }
            }

            return obj;
        }

        private static object GetValue(object source, string name)
        {
            if (source == null)
                return null;
            var type = source.GetType();

            while (type != null)
            {
                var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (f != null)
                    return f.GetValue(source);

                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null)
                    return p.GetValue(source, null);

                type = type.BaseType;
            }

            return null;
        }

        private static object GetArrayValue(object source, string name, int index)
        {
            var enumerable = GetValue(source, name) as System.Collections.IEnumerable;
            if (enumerable == null) return null;
            var enm = enumerable.GetEnumerator();

            for (int i = 0; i <= index; i++)
            {
                if (!enm.MoveNext()) return null;
            }

            return enm.Current;
        }

        /// <summary>
        /// Returns the value of the given property (works like a cast to type).
        /// <para>
        /// Improved from HiddenMonk's functions (http://answers.unity3d.com/questions/627090/convert-serializedproperty-to-custom-class.html)
        /// </para>
        /// </summary>
        public static T CastTo<T>(this SerializedProperty property)
        {
            object obj = (object)property.serializedObject.targetObject;
            string[] strArray = property.propertyPath.Split('.');
            int length = strArray.Length;
            for (int index = 0; index < length; ++index)
            {
                string fieldName = strArray[index];
                if (fieldName == "Array")
                {
                    if (index < length - 2)
                    {
                        string str1 = strArray[index + 1];
                        string str2 = str1.Substring(str1.IndexOf("[") + 1);
                        string str3 = str2.Substring(0, str2.Length - 1);
                        obj = ((IList)obj)[Convert.ToInt32(str3)];
                        ++index;
                    } else
                    {
                        int indexInArray = property.GetIndexInArray();
                        IList<T> objList = (IList<T>)obj;
                        if (objList.Count - 1 >= indexInArray)
                            return objList[indexInArray];
                        return default(T);
                    }
                } else
                    obj = GetFieldOrPropertyValue<object>(fieldName, obj);
            }

            return (T)obj;
        }

        /// <summary>Returns TRUE if this property is inside an array</summary>
        public static bool IsArrayElement(this SerializedProperty property)
        {
            return property.propertyPath.Contains("Array");
        }

        /// <summary>
        /// Returns -1 if the property is not inside an array, otherwise returns its index inside the array
        /// </summary>
        public static int GetIndexInArray(this SerializedProperty property)
        {
            if (!property.IsArrayElement())
                return -1;
            int startIndex = property.propertyPath.LastIndexOf('[') + 1;
            int length = property.propertyPath.LastIndexOf(']') - startIndex;
            return int.Parse(property.propertyPath.Substring(startIndex, length));
        }

        private static T GetFieldOrPropertyValue<T>(string fieldName, object obj)
        {
            System.Type type = obj.GetType();
            System.Reflection.FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            if ((object)field != null)
                return (T)field.GetValue(obj);
            PropertyInfo property = type.GetProperty(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            if ((object)property != null)
                return (T)property.GetValue(obj, (object[])null);
            return default(T);
        }
    }
}