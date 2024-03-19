using Better.Internal.Core.Runtime;
using Better.ProjectSettings.EditorAddons;
using Better.UISystem.Runtime;
using Better.UISystem.Runtime.PopupsSystem;
using Better.UISystem.Runtime.ScreensSystem;
using UnityEditor;

namespace Better.UISystem.EditorAddons.Settings
{
    public class PopupSettingsProvider : DefaultProjectSettingsProvider<PopupSystemSettings>
    {
        public PopupSettingsProvider() : base(PopupSystemSettings.Path)
        {
            keywords = new[] { "popup", "ui", "ui system"};
        }
        
        [MenuItem(PopupSystemSettings.Path + "/" + PrefixConstants.HighlightPrefix, false, 999)]
        private static void Highlight()
        {
            SettingsService.OpenProjectSettings(ProjectPath + PopupSystemSettings.Path);
        }
    }
}