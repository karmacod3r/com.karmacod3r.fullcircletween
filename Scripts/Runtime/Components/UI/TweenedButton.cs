using System;
using FullCircleTween.Properties;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FullCircleTween.Components.UI
{
    [ExecuteAlways]
    public class TweenedButton : Selectable, IPointerClickHandler
    {
        public UnityEvent onClick;
        
        [SerializeField] private TweenStateMachine stateMachine;

        protected override void OnEnable()
        {
            if (stateMachine == null && !Application.isPlaying)
            {
                stateMachine = GetComponent<TweenStateMachine>();
                if (stateMachine == null)
                {
                    stateMachine = gameObject.AddComponent<TweenStateMachine>();
                    stateMachine.Remove("Visible");
                    stateMachine.Remove("Hidden");
                    
                    stateMachine.Add("Normal", new TweenGroupClip());
                    stateMachine.Add("Highlighted", new TweenGroupClip());
                    stateMachine.Add("Pressed", new TweenGroupClip());
                    stateMachine.Add("Selected", new TweenGroupClip());
                    stateMachine.Add("Disabled", new TweenGroupClip());
                }

                transition = Transition.None;
            }

            base.OnEnable();
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            switch (state)
            {
                case SelectionState.Normal:
                    ApplyTweenState("Normal");
                    break;
                case SelectionState.Highlighted:
                    ApplyTweenState("Highlighted");
                    break;
                case SelectionState.Pressed:
                    ApplyTweenState("Pressed");
                    break;
                case SelectionState.Selected:
                    ApplyTweenState("Selected");
                    break;
                case SelectionState.Disabled:
                    ApplyTweenState("Disabled");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void ApplyTweenState(string value)
        {
            if (!stateMachine) return;

            stateMachine.CurrentState = value;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke();
        }
    }
}