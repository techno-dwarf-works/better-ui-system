using System;
using Better.Attributes.Runtime;
using Better.Attributes.Runtime.Manipulation;
using Better.Attributes.Runtime.Select;
using Better.Extensions.Runtime;
using Better.Internal.Core.Runtime;
using Better.ProjectSettings.Runtime;
using Better.Singletons.Runtime.Attributes;
using Better.UISystem.Runtime.PopupsSystem.Popups;
using UnityEditor;
using UnityEngine;

namespace Better.UISystem.Runtime.PopupsSystem
{
    [ScriptableCreate(Path)]
    public class PopupSystemSettings : ScriptableSettings<PopupSystemSettings>
    {
        public const string Path = PrefixConstants.BetterPrefix + "/" + "Popups System";
        private readonly PopupsSequence _fallbackSequence = new SimplePopupsSequence();

        [Header("SETUP")] [Select] [SerializeReference]
        private PopupsSequence _defaultSequence = new SimplePopupsSequence();

        [Select] [SerializeReference] private PopupsSequence[] _overridenSequences;

        [Header("SCREENS")] [SerializeField, ReadOnly]
        private Popup[] _popupPrefabs;

        public Popup[] PopupPrefabs => _popupPrefabs;

        public bool TryGetOverridenSequence(Type sequenceType, out PopupsSequence sequence)
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

        public PopupsSequence GetDefaultSequence()
        {
            if (_defaultSequence == null)
            {
                var message = $"{nameof(_defaultSequence)} is null, returned {nameof(_fallbackSequence)}({_fallbackSequence})";
                Debug.LogWarning(message);

                return _fallbackSequence;
            }

            return _defaultSequence;
        }

        #region Editor

#if UNITY_EDITOR

        [EditorButton("CACHE PREFABS")]
        private void CacheScreenPrefabsEditor()
        {
            var serializedObject = new SerializedObject(this);
            _popupPrefabs = AssetDatabaseUtility.FindPrefabsOfType<Popup>();
            EditorUtility.SetDirty(this);
            serializedObject.ApplyModifiedProperties();
        }
#endif

        #endregion
    }
}