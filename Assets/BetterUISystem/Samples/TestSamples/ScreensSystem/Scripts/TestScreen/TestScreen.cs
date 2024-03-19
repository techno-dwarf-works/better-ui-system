using System.Threading.Tasks;
using Better.UISystem.Runtime.ScreensSystem.Interfaces;
using Better.UISystem.Runtime.ScreensSystem.Screens;
using Samples.TestSamples.ScreensSystem.Scripts;

namespace Samples.TestSamples.ScreensSystem
{
    public class TestScreen : Screen<TestModel, TestView>, IStackingProvider, IStackable
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

        public Task PopStackAsync()
        {
            return ScreenTesterUtility.GetRandomWaitTask();
        }

        public Task PushStackAsync()
        {
            return ScreenTesterUtility.GetRandomWaitTask();
        }

        public Task ReleasedFormStackAsync()
        {
            return ScreenTesterUtility.GetRandomWaitTask();
        }
    }
}