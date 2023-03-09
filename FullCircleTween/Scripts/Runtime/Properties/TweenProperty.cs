using System;
using UnityEngine;

namespace FullCircleTween.Properties
{
    [Serializable]
    public class TweenProperty
    {
        [SerializeField] private string typeName;
        [SerializeField] public string tweenMethodName;
    }
}