using UnityEngine;

namespace FullCircleTween.Attributes
{
    public class DynamicDropDownAttribute : PropertyAttribute
    {
        public string optionsMemberName;
        public string onChangeListenerName;
        public bool allowUndefined;

        public DynamicDropDownAttribute(string optionsMemberName, string onChangeListenerName = "", bool allowUndefined = true)
        {
            this.optionsMemberName = optionsMemberName;
            this.onChangeListenerName = onChangeListenerName;
        }
    }
}