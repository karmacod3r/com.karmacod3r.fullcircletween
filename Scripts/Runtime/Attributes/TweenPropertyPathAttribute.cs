using System;

namespace FullCircleTween.Attributes
{
    public class TweenPropertyPathAttribute: Attribute
    {
        public string propertyPath;
        
        public TweenPropertyPathAttribute(string propertyPath)
        {
            this.propertyPath = propertyPath;
        }
    }
}