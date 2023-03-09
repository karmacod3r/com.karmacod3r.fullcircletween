using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using FullCircleTween.Attributes;
using FullCircleTween.Components;
using FullCircleTween.Core;
using FullCircleTween.Core.Interfaces;
using FullCircleTween.Extensions;
using FullCircleTween.Properties;
using FullCircleTween.TweenCollections;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace FullCircleTween.Test
{
    [ExecuteAlways]
    [TweenCollection]
    public class TweenTest : MonoBehaviour
    {
        [OnValueChanged(nameof(OnTweenTargetChanged))]
        public TweenTarget tweenTarget;

        private void OnTweenTargetChanged()
        {
            var tweenTest = tweenTarget.GetValue(this) as TweenTest;
            if (tweenTest == null) return;
            Debug.Log(tweenTest.floatValue);
        }

        [SerializeField, ProgressBar(1)] private float floatValue;

        public Tween<float> TweenFloatValue(float toValue, float duration)
            => Tween<float>.To(this, () => floatValue, value => floatValue = value, toValue, duration);

        [Button]
        private void FloatTest()
        {
            floatValue = 0;
            Tween<float>.To(this, () => floatValue, value => floatValue = value, 1, 1);
        }

        [SerializeField, ProgressBar(100)] private int intValue;

        [Button]
        private void IntTest()
        {
            intValue = 0;
            Tween<int>.To(this, () => intValue, value => intValue = value, 100, 1);
        }

        [Button]
        private void LocalPositionTest()
        {
            transform.KillTweens();
            transform.TweenLocalPositionY(10f, 1).SetEasing(Ease.OutCubic)
                .Then(() => transform.TweenLocalPositionY(0, 1).SetEasing(Ease.InCubic).SetDelay(1f));
        }

        [Button]
        private void CoroutineTest()
        {
            StartCoroutine(WaitHalfASecond());
            StartCoroutine(WaitForOneSecond());
        }

        private IEnumerator WaitHalfASecond()
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log("Waited 0.5s");
        }

        private IEnumerator WaitForOneSecond()
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("Waited 1s");
        }

        [Button]
        private void SerializationTest()
        {
            var v = new TweenClip();
            var buffer = Serializer.SerializeObject(v);
            Debug.Log(Serializer.DeserializeObject<TweenClip>(buffer));
        }
    }
}