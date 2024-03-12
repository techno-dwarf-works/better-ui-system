using System.Threading.Tasks;

namespace Better.UISystem.Runtime.ScreensSystem.Interfaces
{
    public interface IStackable : IScreen
    {
        public Task PopStackAsync();
        public Task PushStackAsync();
        public Task ReleasedFormStackAsync();
    }
}