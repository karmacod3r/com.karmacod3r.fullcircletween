using UnityEngine;

namespace FullCircleTween.Attributes
{
    public class DropDownAttribute : PropertyAttribute
    {
        public string optionsMemberName;
        public string placeholder;
        public string onChangeListenerName;
        public bool allowUndefined;

        public DropDownAttribute(string optionsMemberName, string onChangeListenerName = "", string placeholder = "", bool allowUndefined = true)
        {
            this.optionsMemberName = optionsMemberName;
            this.placeholder = placeholder;
            this.onChangeListenerName = onChangeListenerName;
            this.allowUndefined = allowUndefined;
        }
    }
}