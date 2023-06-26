using System;
using UnityEngine;

namespace FullCircleTween.Core
{
    [DefaultExecutionOrder(-20000)]
    public class UpdateDispatcher : MonoBehaviour
    {
        private void Update()
        {
            TweenManager.UpdateTweens(Time.deltaTime);
        }

        private void OnDestroy()
        {
            TweenManager.KillAll();
        }
    }
}