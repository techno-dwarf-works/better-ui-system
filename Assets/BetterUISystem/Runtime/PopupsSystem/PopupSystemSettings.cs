using System;
using Better.Attributes.Runtime.Manipulation;
using Better.Attributes.Runtime.Select;
using Better.ProjectSettings.Runtime;
using UnityEngine;

namespace Better.UISystem.Runtime.PopupsSystem
{
    public abstract class PopupSystemSettings<TSettings, TDerived, TSequence> : ScriptableSettings<TSettings> 
        where TSettings : ScriptableSettings<TSettings>
    {
        [Header("SETUP")] [Select] [SerializeReference]
        private TSequence _defaultSequence;

        [Select] [SerializeReference] private TSequence[] _overridenSequences;

        [Header("SCREENS")] [SerializeField, ReadOnly]
        protected TDerived[] _prefabs;

        public TDerived[] Prefabs => _prefabs;

        public abstract TSequence FallbackSequence { get; }

        public bool TryGetOverridenSequence(Type sequenceType, out TSequence sequence)
        {
            if (sequenceType != null)
            {
                for (var i = 0; i < _overridenSequences.Length; i++)
                {
                    sequence = _overridenSequences[i];
                    if (sequence.GetType() == sequenceType)
                    {
                        return true;
                    }
                }
            }

            sequence = default;
            return false;
        }

        public TSequence GetDefaultSequence()
        {
            if (_defaultSequence == null)
            {
                var message = $"{nameof(_defaultSequence)} is null, returned {nameof(FallbackSequence)}({FallbackSequence})";
                Debug.LogWarning(message);

                return FallbackSequence;
            }

            return _defaultSequence;
        }
    }
}