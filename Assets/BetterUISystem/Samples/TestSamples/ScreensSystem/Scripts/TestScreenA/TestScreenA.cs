using System.Threading.Tasks;
using Better.UISystem.Runtime.ScreensSystem.Interfaces;
using Better.UISystem.Runtime.ScreensSystem.Screens;
using Samples.TestSamples.ScreensSystem.Scripts;

namespace Samples.TestSamples.ScreensSystem
{
    public class TestScreenA : Screen<TestModelA, TestViewA>, IStackingProvider
    {
        protected override Task OnInitializeAsync()
        {
            return ScreenTesterUtility.GetRandomWaitTask();
        }

        protected override Task OnPrepareShowAsync()
        {
            return ScreenTesterUtility.GetRandomWaitTask();
        }

        protected override Task OnShowAsync()
        {
            return ScreenTesterUtility.GetRandomWaitTask();
        }

        protected override void Rebuild()
        {
        }

        protected override Task OnPrepareHideAsync()
        {
            return ScreenTesterUtility.GetRandomWaitTask();
        }

        protected override Task OnHideAsync()
        {
            return ScreenTesterUtility.GetRandomWaitTask();
        }
    }
}