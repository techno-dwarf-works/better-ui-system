#if BETTER_SERVICES
using System.Threading;
using System.Threading.Tasks;
using Better.Services.Runtime;
using Better.UISystem.Runtime.ScreensSystem.Implementations;
using Better.UISystem.Runtime.ScreensSystem.Interfaces;
using Better.UISystem.Runtime.ScreensSystem.Screens;
using Better.UISystem.Runtime.ScreensSystem.Transitions;
using UnityEngine;

namespace Better.UISystem.Runtime.ScreensSystem
{
    [RequireComponent(typeof(RectTransform))]
    public class ScreenService : MonoService, IScreenSystem
    {
        private ISceneSystem _internalSystem;

        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            var rectTransform = GetComponent<RectTransform>();
            _internalSystem = new InternalScreenSystem(rectTransform);
            return Task.CompletedTask;
        }

        protected override Task OnPostInitializeAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #region IScreensSystem

        public int HistoryCount => _internalSystem.HistoryCount;

        public RectTransform RectTransform => _internalSystem.RectTransform;

        public ScreenTransitionInfo<TPresenter, TModel> CreateTransition<TPresenter, TModel>(TModel model, CancellationToken cancellationToken = default) where TPresenter : Screen<TModel> where TModel : ScreenModel
        {
            return _internalSystem.CreateTransition<TPresenter, TModel>(model, cancellationToken);
        }

        public HistoryTransitionInfo CreateHistoryTransition(int historyDepth = 1, CancellationToken cancellationToken = default)
        {
            return _internalSystem.CreateHistoryTransition(historyDepth, cancellationToken);
        }

        public bool IsOpened<TPresenter, TModel>() where TPresenter : Screen<TModel> where TModel : ScreenModel
        {
            return _internalSystem.IsOpened<TPresenter, TModel>();
        }

        public bool TryGetOpened<TPresenter, TModel>(out TPresenter screen) where TPresenter : Screen<TModel> where TModel : ScreenModel
        {
            return _internalSystem.TryGetOpened<TPresenter, TModel>(out screen);
        }

        #endregion
    }
}
#endif