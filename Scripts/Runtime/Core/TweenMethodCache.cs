using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FullCircleTween.Attributes;
using FullCircleTween.Core.Interfaces;
using FullCircleTween.Properties;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace FullCircleTween.Core
{
    public static class TweenMethodCache
    {
        public static List<string> allComponentTypeNames = new List<string>();
        private static Dictionary<Type, Dictionary<string, MethodInfo>> tweeenMethods = new Dictionary<Type, Dictionary<string, MethodInfo>>();
        private static Dictionary<Type, List<string>> popupListCache = new Dictionary<Type, List<string>>();

        private static readonly Dictionary<Type, Type> unityTypeMap = new Dictionary<Type, Type>
        {
            {typeof(Single), typeof(float)}
        };


#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void EditorInitialize()
        {
            if (Application.isPlaying) return;
            RecacheTweenMethods();
        }
#endif

        [RuntimeInitializeOnLoadMethod]
        private static void RuntimeInitialize()
        {
            if (!Application.isPlaying) return;
            RecacheTweenMethods();
        }

        public static void RecacheTweenMethods()
        {
            tweeenMethods.Clear();
            popupListCache.Clear();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            assemblies.ForEach(assembly =>
            {
                assembly.GetTypes()
                    .Where(type => type.GetCustomAttribute<TweenCollectionAttribute>() != null)
                    .ToList()
                    .ForEach(AddTweenMethods);
            });

            allComponentTypeNames = tweeenMethods.Keys.ToList()
                .Where(type => typeof(Component).IsAssignableFrom(type))
                .Select(type => type.AssemblyQualifiedName)
                .ToList();
            allComponentTypeNames.Sort();
        }

        private static void AddTweenMethods(Type extensionClass)
        {
            extensionClass.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly).ToList().ForEach(info =>
            {
                if (!typeof(ITween).IsAssignableFrom(info.ReturnType)) return;

                var parameters = info.GetParameters();
                var targetType = info.IsStatic ? parameters[0].ParameterType : extensionClass;

                if (!tweeenMethods.ContainsKey(targetType))
                {
                    tweeenMethods.Add(targetType, new Dictionary<string, MethodInfo>());
                }

                var methodName = GetMethodName(info);
                if (tweeenMethods[targetType].ContainsKey(methodName))
                {
                    Debug.LogError($"Tween method already defined - skipping {methodName} ({info}) for type {targetType}");
                    return;
                }

                tweeenMethods[targetType][methodName] = info;
            });
        }

        private static string GetFriendlyTypeName(Type type)
        {
            var ret = type.ToString();
            
            ret = ret switch
            {
                "System.Single" => "float",
                "System.Int32" => "int",
                "System.Bool" => "bool",
                "System.String" => "string",
                _ => ret
            };

            return ret.StartsWith("UnityEngine.") ? ret.Substring(12) : ret;
        }

        private static string GetMethodTweenedTypeName(MethodInfo info)
        {
            return GetFriendlyTypeName(GetMethodTweenedType(info));
        }

        /*
        public static string ExpandMethodName(string memberName, Type tweenedType)
        {
            
        }
        */

        public static string GetMethodName(MethodInfo info)
        {
            return info.Name + "(" + GetMethodTweenedTypeName(info) + ")";
        }

        public static Type GetMethodTweenedType(MethodInfo info)
        {
            if (info == null) return null;

            var parameterType = info.GetParameters()[info.IsStatic ? 1 : 0].ParameterType;
            return unityTypeMap.ContainsKey(parameterType) ? unityTypeMap[parameterType] : parameterType;
        }

        public static List<string> GetPopupMethodNames(Type targetType)
        {
            if (popupListCache.ContainsKey(targetType)) return popupListCache[targetType];

            var ret = tweeenMethods.ContainsKey(targetType) ? tweeenMethods[targetType].Keys.ToList() : new List<string>();
            ret.Sort();
            popupListCache[targetType] = ret;
            return ret;
        }

        public static MethodInfo GetTweenMethodInfo(Type targetType, string methodName)
        {
            if (!tweeenMethods.ContainsKey(targetType) || !tweeenMethods[targetType].ContainsKey(methodName)) return null;
            return tweeenMethods[targetType][methodName];
        }

        public static ITween CreateTween(object target, string methodName, TweenClipValue toValue, float duration)
        {
            var methodInfo = GetTweenMethodInfo(target.GetType(), methodName);
            if (methodInfo == null) return null;

            var tweenedType = GetMethodTweenedType(methodInfo);

            var parameters = methodInfo.GetParameters();
            if (methodInfo.IsStatic && parameters.Length == 3)
            {
                return (ITween) methodInfo.Invoke(target, new[] {target, toValue.GetValue(tweenedType), duration});
            }

            if (!methodInfo.IsStatic && parameters.Length == 2)
            {
                return (ITween) methodInfo.Invoke(target, new[] {toValue.GetValue(tweenedType), duration});
            }

            return null;
        }
    }
}