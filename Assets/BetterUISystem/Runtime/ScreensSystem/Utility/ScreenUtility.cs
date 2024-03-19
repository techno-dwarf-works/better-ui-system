using System.Threading;
using System.Threading.Tasks;
using Better.Extensions.Runtime;
using Better.UISystem.Runtime.ScreensSystem.Interfaces;
using Better.UISystem.Runtime.ScreensSystem.Screens;

namespace Better.UISystem.Runtime.ScreensSystem.Utility
{
    public static class ScreenUtility
    {
#if BETTER_SERVICES && BETTER_LOCATOR
        private static readonly Locators.Runtime.ServiceProperty<ScreenService> _serviceProperty = new();
#endif

        public static IScreenSystem GetSystem()
        {
#if BETTER_SERVICES && BETTER_LOCATOR
            if (_serviceProperty.IsRegistered)
            {
                return _serviceProperty.CachedService;
            }
#endif

#if BETTER_SINGLETONS
            return ScreenManager.Instance;
#endif

#pragma warning disable CS0162
            return new ScreenSystem();
#pragma warning restore CS0162
        }
        
        public static bool IsOpened<TPresenter, TModel>()
            where TPresenter : Screen<TModel>
            where TModel : ScreenModel
        {
            return GetSystem().IsOpened<TPresenter, TModel>();
        }

        public static bool TryGetOpened<TPresenter, TModel>(out TPresenter screen)
            where TPresenter : Screen<TModel>
            where TModel : ScreenModel
        {
            return GetSystem().TryGetOpened<TPresenter, TModel>(out screen);
        }

        public static Task<TPresenter> OpenAsync<TPresenter, TModel>(TModel model, CancellationToken cancellationToken = default)
            where TPresenter : Screen<TModel>
            where TModel : ScreenModel
        {
            return GetSystem()
                .CreateTransition<TPresenter, TModel>(model, cancellationToken)
                .RunAsync();
        }

        public static void Open<TPresenter, TModel>(TModel model)
            where TPresenter : Screen<TModel>
            where TModel : ScreenModel
        {
            OpenAsync<TPresenter, TModel>(model).Forget();
        }

        public static Task OpenHistoryAsync(int historyDepth = 1, CancellationToken cancellationToken = default)
        {
            return GetSystem()
                .CreateHistoryTransition(historyDepth, cancellationToken)
                .RunAsync();
        }

        public static void OpenHistory(int historyDepth = 1, CancellationToken cancellationToken = default)
        {
            OpenHistoryAsync(historyDepth, cancellationToken).Forget();
        }
    }
}