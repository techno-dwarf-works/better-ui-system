using System.Threading.Tasks;
using Better.UISystem.Runtime.Common;
using Better.UISystem.Runtime.ScreensSystem.Screens;
using Better.UISystem.Runtime.ScreensSystem.Transitions;

namespace Better.UISystem.Runtime.ScreensSystem.Interfaces
{
    public interface IScreenTransitionRunner : ITransitionRunner
    {
        public Task<TPresenter> RunAsync<TPresenter, TModel>(ScreenTransitionInfo<TPresenter, TModel> info)
            where TPresenter : Screen<TModel>
            where TModel : ScreenModel;

        public Task RunAsync(HistoryTransitionInfo info);
    }
}