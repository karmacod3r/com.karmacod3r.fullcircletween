using System;
using System.Globalization;
using FullCircleTween.Core;
using FullCircleTween.Core.Interfaces;
using UnityEngine;

namespace FullCircleTween.Properties
{
    [Serializable]
    public class TweenClipValue
    {
        [SerializeField] public string serializedValue;
        
        public void SetValue(object value)
        {
            serializedValue = Serialize(value);
        }

        public object GetValue(Type valueType) => Deserialize(valueType, serializedValue);

        public TweenClipValue()
        {
        }
        
        public TweenClipValue(object value)
        {
            SetValue(value);
        }

        public static string Serialize(object value)
        {
            return Serializer.SerializeObject(value);
        }

        public static object Deserialize(Type valueType, string serializedValue)
        {
            return Serializer.DeserializeObject(valueType, serializedValue);
        }

        private static T DeserializeJson<T>(string json, T defaultValue)
        {
            if (string.IsNullOrEmpty(json)) return defaultValue;
            
            try
            {
                return JsonUtility.FromJson<T>(json);
            } catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}