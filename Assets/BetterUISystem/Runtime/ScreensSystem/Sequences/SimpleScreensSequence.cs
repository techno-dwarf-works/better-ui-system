using System;
using System.Threading.Tasks;
using Better.UISystem.Runtime.ScreensSystem.Interfaces;

namespace Better.UISystem.Runtime.ScreensSystem.Sequences
{
    [Serializable]
    public class SimpleScreensSequence : ScreensSequence
    {
        public override async Task DoPlay(IScreen to)
        {
            await to.PrepareShowAsync();
            await to.ShowAsync();
        }

        public override async Task DoPlay(IScreen from, IScreen to)
        {
            await Task.WhenAll(
                from.PrepareHideAsync(),
                to.PrepareShowAsync()
            );

            await from.HideAsync();
            await to.ShowAsync();
        }

        public override ScreensSequence GetInverseSequence()
        {
            return this;
        }
    }
}