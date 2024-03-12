using System.Threading;
using Better.UISystem.Runtime.ScreensSystem.Screens;
using Better.UISystem.Runtime.ScreensSystem.Transitions;
using UnityEngine;

namespace Better.UISystem.Runtime.ScreensSystem.Interfaces
{
    public interface IScreenSystem
    {
        public int HistoryCount { get; }
        
        public RectTransform Container { get; }

        public ScreenTransitionInfo<TPresenter, TModel> CreateTransition<TPresenter, TModel>(TModel model, CancellationToken cancellationToken = default)
            where TPresenter : Screen<TModel>
            where TModel : ScreenModel;

        public HistoryTransitionInfo CreateHistoryTransition(int historyDepth = 1, CancellationToken cancellationToken = default);
        
        public bool IsOpened<TPresenter, TModel>() where TPresenter : Screen<TModel> where TModel : ScreenModel;
        
        public bool TryGetOpened<TPresenter, TModel>(out TPresenter screen) where TPresenter : Screen<TModel> where TModel : ScreenModel;

    }
}