using System;
using System.Text.RegularExpressions;
using FullCircleTween.Attributes;
using UnityEngine;

namespace FullCircleTween.Properties
{
    [Serializable]
    public class TweenState
    {
        public enum TweenStateMatchType
        {
            Equal,
            NotEqual,
            RegexMatch,
            RegexNoMatch,
            CatchAll
        }
        
        public string stateName;
        public TweenStateMatchType matchType;
        public string stateRegex;
        public TweenGroupClip tweenGroup;
        
        private string stateRegexCompiledSource;
        private Regex stateRegexCompiled;
        
        public bool StateNameMatches(string name)
        {
            if (matchType == TweenStateMatchType.CatchAll)
            {
                return true;
            }

            if (matchType == TweenStateMatchType.RegexMatch || matchType == TweenStateMatchType.RegexNoMatch)
            {
                if (stateRegexCompiled == null || stateRegexCompiledSource != stateRegex)
                {
                    stateRegexCompiledSource = stateRegex;
                    try
                    {
                        stateRegexCompiled = new Regex(stateRegex);
                    }
                    catch (ArgumentException)
                    {
                        Debug.LogError($"Regex ({stateRegex}) is invalid!");
                        return false;
                    }
                }

                var match = stateRegexCompiled.IsMatch(name);
                return matchType == TweenStateMatchType.RegexMatch ? match : !match;
            }

            return matchType == TweenStateMatchType.Equal ? stateName == name : stateName != name;
        }
    }
}