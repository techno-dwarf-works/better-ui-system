using System.Threading;
using System.Threading.Tasks;
using Better.Extensions.Runtime;
using Better.UISystem.Runtime.ScreensSystem.Interfaces;
using Better.UISystem.Runtime.ScreensSystem.Screens;

namespace Better.UISystem.Runtime.ScreensSystem.Utility
{
    public static class ScreenSystemExtensions
    {
        public static bool IsOpened<TPresenter, TModel>(this IScreenSystem self)
            where TPresenter : Screen<TModel>
            where TModel : ScreenModel
        {
            return self.IsOpened<TPresenter, TModel>();
        }

        public static bool TryGetOpened<TPresenter, TModel>(this IScreenSystem self, out TPresenter screen)
            where TPresenter : Screen<TModel>
            where TModel : ScreenModel
        {
            return self.TryGetOpened<TPresenter, TModel>(out screen);
        }

        public static Task<TPresenter> OpenAsync<TPresenter, TModel>(this IScreenSystem self, TModel model, CancellationToken cancellationToken = default)
            where TPresenter : Screen<TModel>
            where TModel : ScreenModel
        {
            return self
                .CreateTransition<TPresenter, TModel>(model, cancellationToken)
                .RunAsync();
        }

        public static void Open<TPresenter, TModel>(this IScreenSystem self, TModel model)
            where TPresenter : Screen<TModel>
            where TModel : ScreenModel
        {
            self.OpenAsync<TPresenter, TModel>(model).Forget();
        }

        public static Task OpenHistoryAsync(this IScreenSystem self, int historyDepth = 1, CancellationToken cancellationToken = default)
        {
            return self
                .CreateHistoryTransition(historyDepth, cancellationToken)
                .RunAsync();
        }

        public static void OpenHistory(this IScreenSystem self, int historyDepth = 1, CancellationToken cancellationToken = default)
        {
            OpenHistoryAsync(self, historyDepth, cancellationToken).Forget();
        }
    }
}