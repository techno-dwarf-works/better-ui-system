using System;
using System.Linq;
using Better.Attributes.Runtime;
using Better.Attributes.Runtime.Manipulation;
using Better.Attributes.Runtime.Select;
using Better.Extensions.Runtime;
using Better.Internal.Core.Runtime;
using Better.ProjectSettings.Runtime;
using Better.Singletons.Runtime.Attributes;
using Better.UISystem.Runtime.ScreensSystem.Interfaces;
using Better.UISystem.Runtime.ScreensSystem.Sequences;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Better.UISystem.Runtime.ScreensSystem
{
    [ScriptableCreate(Path)]
    public class ScreensSystemSettings : ScriptableSettings<ScreensSystemSettings>
    {
        public const string Path = PrefixConstants.BetterPrefix + "/" + "Screens System";
        private readonly ScreensSequence _fallbackSequence = new SimpleScreensSequence();

        [Header("SETUP")] [Select] [SerializeReference]
        private ScreensSequence _defaultSequence = new SimpleScreensSequence();

        [Select] [SerializeReference] private ScreensSequence[] _overridenSequences;

        [Header("SCREENS")] [SerializeField, ReadOnly]
        private Component[] _screenPrefabs;

        public Component[] ScreenPrefabs => _screenPrefabs;

        public bool TryGetOverridenSequence(Type sequenceType, out ScreensSequence sequence)
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

            sequence = null;
            return false;
        }

        public ScreensSequence GetDefaultSequence()
        {
            if (_defaultSequence == null)
            {
                var message = $"{nameof(_defaultSequence)} is null, returned {nameof(_fallbackSequence)}({_fallbackSequence})";
                Debug.LogWarning(message);

                return _fallbackSequence;
            }

            return _defaultSequence;
        }

        public bool TryGetHistorySequence(Type sequenceType, out ScreensSequence sequence)
        {
            for (var i = 0; i < _overridenSequences.Length; i++)
            {
                sequence = _overridenSequences[i];
                if (sequence.GetType() == sequenceType)
                {
                    return true;
                }
            }

            sequence = null;
            return false;
        }

        #region Editor

#if UNITY_EDITOR

        [EditorButton("CACHE PREFABS")]
        private void CacheScreenPrefabsEditor()
        {
            var serializedObject = new SerializedObject(this);
            var rawPrefabs = AssetDatabaseUtility.FindPrefabsOfType<IScreen>();
            _screenPrefabs = rawPrefabs.Select(p => (Component)p).ToArray();
            EditorUtility.SetDirty(this);
            serializedObject.ApplyModifiedProperties();
        }
#endif

        #endregion
    }
}