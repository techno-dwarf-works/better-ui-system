using Better.Internal.Core.Runtime;
using Better.ProjectSettings.EditorAddons;
using Better.UISystem.Runtime.ScreensSystem;
using UnityEditor;

namespace Better.UISystem.EditorAddons.Settings
{
    public class ScreensSettingsProvider : DefaultProjectSettingsProvider<ScreensSystemSettings>
    {
        public ScreensSettingsProvider() : base(ScreensSystemSettings.Path)
        {
            keywords = new[] { "screen", "ui", "ui system"};
        }
        
        [MenuItem(ScreensSystemSettings.Path + "/" + PrefixConstants.HighlightPrefix, false, 999)]
        private static void Highlight()
        {
            SettingsService.OpenProjectSettings(ProjectPath + ScreensSystemSettings.Path);
        }
    }
}