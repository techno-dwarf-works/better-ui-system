using Better.Internal.Core.Runtime;
using Better.ProjectSettings.EditorAddons;
using Better.UISystem.Runtime.ScreensSystem;
using UnityEditor;

namespace Better.UISystem.EditorAddons.Settings
{
    public class ScreenSettingsProvider : DefaultProjectSettingsProvider<ScreenSystemSettings>
    {
        public ScreenSettingsProvider() : base(ScreenSystemSettings.Path)
        {
            keywords = new[] { "screen", "ui", "ui system"};
        }
        
        [MenuItem(ScreenSystemSettings.Path + "/" + PrefixConstants.HighlightPrefix, false, 999)]
        private static void Highlight()
        {
            SettingsService.OpenProjectSettings(ProjectPath + ScreenSystemSettings.Path);
        }
    }
}