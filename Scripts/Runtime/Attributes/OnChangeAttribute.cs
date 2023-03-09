using UnityEngine;

namespace FullCircleTween.Attributes
{
    public class OnChangeAttribute : PropertyAttribute
    {
        public string listenerName;

        public OnChangeAttribute(string listenerName)
        {
            this.listenerName = listenerName;
        }
    }
}