using Better.Internal.Core.Runtime;
using Better.ProjectSettings.EditorAddons;
using Better.UISystem.Runtime;
using UnityEditor;

namespace Better.UISystem.EditorAddons.Settings
{
    public class ModalSettingsProvider : DefaultProjectSettingsProvider<ModalSystemSettings>
    {
        public ModalSettingsProvider() : base(ModalSystemSettings.Path)
        {
            keywords = new[] { "modal", "ui", "ui system"};
        }
        
        [MenuItem(ModalSystemSettings.Path + "/" + PrefixConstants.HighlightPrefix, false, 999)]
        private static void Highlight()
        {
            SettingsService.OpenProjectSettings(ProjectPath + ModalSystemSettings.Path);
        }
    }
}