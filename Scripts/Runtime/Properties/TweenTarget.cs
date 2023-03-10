using System;
using System.Linq;
using UnityEngine;

namespace FullCircleTween.Properties
{
    [Serializable]
    public class TweenTarget
    {
        [SerializeField] public string typeName;
        
        public TweenTarget() {}

        public TweenTarget(Component target)
        {
            typeName = Serialize(target);
        }

        public Component GetValue(Component context)
            => Deserialize(context, typeName);
        
        public void SetValue(Component target)
            => typeName = Serialize(target);
        
        public static string Serialize(Component component)
        {
            var ret = component.GetType().AssemblyQualifiedName;
            var sameTypeSiblings = component.transform.GetComponents(component.GetType());
            var index = sameTypeSiblings.ToList().FindIndex(c => c == component);
            return ret + (sameTypeSiblings.Length > 1 ? $"^{index}" : "");
        }

        public static Component Deserialize(Component context, string assemblyName)
        {
            var parts = assemblyName.Split('^');
            if (parts.Length == 1)
            {
                return context.GetComponent(Type.GetType(assemblyName));
            }

            var components = context.GetComponents(Type.GetType(parts[0]));
            var index = int.Parse(parts[1]);
            if (index < 0 || index >= components.Length) return null;
            
            return components[index];
        }
    }
}