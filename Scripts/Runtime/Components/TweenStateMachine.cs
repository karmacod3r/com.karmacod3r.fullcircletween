using System;
using System.Collections.Generic;
using System.Linq;
using FullCircleTween.Attributes;
using FullCircleTween.Core;
using FullCircleTween.Core.Interfaces;
using FullCircleTween.Extensions;
using FullCircleTween.Properties;
using UnityEditor;
using UnityEngine;

namespace FullCircleTween.Components
{
    public class TweenStateMachine : MonoBehaviour
    {
        public bool controlledByParent = true;
        [SerializeField] private string currentState = "Visible";

        [SerializeField] private List<TweenState> tweenStates = new()
        {
            new() { stateName = "Hidden" },
            new() { stateName = "Visible" }
        };

        public string[] StateNames => tweenStates?.Select(groupClip => groupClip.stateName).ToArray();

        public string CurrentState
        {
            get => currentState;
            set
            {
                if (currentState == value) return;
                currentState = value;
                OnStateChanged();
            }
        }

        private void OnStateChanged()
        {
            ApplyState(currentState);
        }

        private void ApplyState(string stateName)
        {
            var newState = TryGetState(stateName);
            if (newState == null) return;

            KillAll();
            var tweenGroup = newState.Create(this);
            
            foreach (Transform child in transform)
            {
                ApplyChildTweens(child, tweenGroup, stateName);
            }
            newState.Play(this, tweenGroup);
        }

        private void ApplyChildTweens(Transform root, TweenGroup tweenGroup, string stateName)
        {
            var tweenPlayers = root.GetComponents<TweenStateMachine>();

            var stateApplied = false;
            foreach (var tweenPlayer in tweenPlayers)
            {
                if (tweenPlayer == null || tweenPlayer == this) continue;

                stateApplied = stateApplied || tweenPlayer.ApplyStateFromParent(stateName, tweenGroup);
            }

            // Only apply to children, if no other tween players are present or state was applied
            if (tweenPlayers.Length == 0 || stateApplied)
            {
                foreach (Transform child in root)
                {
                    ApplyChildTweens(child, tweenGroup, stateName);
                }
            }
        }

        private bool ApplyStateFromParent(string value, TweenGroup targetTweenGroup)
        {
            if (!controlledByParent) return false;

            var newState = TryGetState(value);
            if (newState == null) return false;

            currentState = value;
            var tween = newState.Play(transform);
            targetTweenGroup.Insert(tween, 0);

            return true;
        }

        public int GetStateIndex(string stateName) => tweenStates.FindIndex(state => state.StateNameMatches(stateName));

        public bool HasState(string stateName) => GetStateIndex(stateName) > -1;

        public TweenState GetState(string stateName)
        {
            return GetState(GetStateIndex(stateName));
        }

        public TweenState GetState(int index)
        {
            return index > -1 && index < tweenStates.Count ? tweenStates[index] : null;
        }

        public void Add(string stateName, TweenGroupClip groupClip)
        {
            if (groupClip == null) return;
            if (HasState(stateName))
            {
                Debug.LogError($"TweenPlayer already contains a tween group for state '{stateName}'");
                return;
            }

            tweenStates.Add(new TweenState
                {
                    stateName = stateName,
                    tweenGroup = groupClip
                }
            );
        }

        public void Remove(string stateName)
        {
            var index = GetStateIndex(stateName);
            if (index == -1) return;

            tweenStates[index].tweenGroup.Pause();
            tweenStates.RemoveAt(index);
        }

        private TweenGroupClip TryGetState(string stateName)
        {
            var index = GetStateIndex(stateName);
            if (index == -1)
            {
                return null;
            }

            return tweenStates[index].tweenGroup;
        }

        private void Play(string stateName)
        {
            TryGetState(stateName)?.Play(this);
        }

        public void KillAll()
        {
            tweenStates.ForEach(state => state.tweenGroup.Kill());
            this.KillAllTweens();
        }

        public void Pause(string stateName)
        {
            TryGetState(stateName)?.Pause();
        }

        private void OnDestroy()
        {
            KillAll();
        }

        private void OnDisable()
        {
            KillAll();
        }

        private int stateIndex;
    }
}