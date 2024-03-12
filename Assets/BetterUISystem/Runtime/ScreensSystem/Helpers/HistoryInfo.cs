using Better.UISystem.Runtime.ScreensSystem.Interfaces;
using Better.UISystem.Runtime.ScreensSystem.Transitions;

namespace Better.UISystem.Runtime.ScreensSystem.Helpers
{
    public class HistoryInfo
    {
        public IStackable Screen { get; }
        public ScreenTransitionInfo Info { get; }

        public HistoryInfo(IStackable screen, ScreenTransitionInfo info)
        {
            Screen = screen;
            Info = info;
        }

        public void Deconstruct(out IStackable screen, out ScreenTransitionInfo info)
        {
            screen = Screen;
            info = Info;
        }
    }
}