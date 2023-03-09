using UnityEngine;

namespace FullCircleTween.Attributes
{
    public class SelectAttribute : PropertyAttribute
    {
        public string optionsMemberName;
        public string onChangeListenerName;
        public bool allowUndefined;

        public SelectAttribute(string optionsMemberName, string onChangeListenerName = "", bool allowUndefined = true)
        {
            this.optionsMemberName = optionsMemberName;
            this.onChangeListenerName = onChangeListenerName;
        }
    }
}