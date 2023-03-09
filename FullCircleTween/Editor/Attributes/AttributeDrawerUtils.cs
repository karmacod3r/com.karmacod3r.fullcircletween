using System.Collections.Generic;
using System.Reflection;

namespace FullCircleTween.Attributes
{
    public static class AttributeDrawerUtils
    {
        internal static IEnumerable<string> GetOptions(object target, string memberName)
        {
            var type = target.GetType();
            object value = null;

            var field = type.GetField(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
            if (field != null)
            {
                value = field.GetValue(target);
            } else
            {
                var property = type.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
                if (property != null)
                {
                    value = property.GetValue(target);
                } else
                {
                    var method = type.GetMethod(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
                    if (method != null && method.ReturnParameter != null && method.GetParameters().Length == 0)
                    {
                        value = method.Invoke(target, null);
                    }
                }
            }

            return value as IEnumerable<string>;
        }        
    }
}