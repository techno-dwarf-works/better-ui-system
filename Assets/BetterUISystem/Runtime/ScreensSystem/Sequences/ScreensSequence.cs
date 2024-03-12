using System;
using System.Threading.Tasks;
using Better.UISystem.Runtime.ScreensSystem.Interfaces;

namespace Better.UISystem.Runtime.ScreensSystem.Sequences
{
    [Serializable]
    public abstract class ScreensSequence
    {
        public abstract Task DoPlay(IScreen to);
        public abstract Task DoPlay(IScreen from, IScreen to);
        public abstract ScreensSequence GetInverseSequence();
    }
}