using System;
using System.Threading;
using Better.UISystem.Runtime.ScreensSystem.Interfaces;
using Better.UISystem.Runtime.ScreensSystem.Screens;
using Better.UISystem.Runtime.ScreensSystem.Transitions;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Better.UISystem.Runtime.ScreensSystem
{
    public class ScreenSystem : IScreenSystem
    {
        private readonly InternalScreenSystem _internalSystem;
        private RectTransform _rectTransform;

        public ScreenSystem()
        {
            var container = FindOrCreateCanvas();
            _internalSystem = new InternalScreenSystem(container);
        }

        private RectTransform FindOrCreateCanvas()
        {
            var canvas = Object.FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                return canvas.GetComponent<RectTransform>();
            }

            var canvasComponents = new Type[]
            {
                typeof(RectTransform), typeof(Canvas), typeof(CanvasRenderer), typeof(CanvasScaler)
            };

            var systemGameObject = new GameObject(nameof(ScreenSystem), canvasComponents);
            return systemGameObject.GetComponent<RectTransform>();
        }

        #region IScreenSystem
        
        public int HistoryCount => _internalSystem.HistoryCount;

        public RectTransform Container => _internalSystem.Container;

        public ScreenTransitionInfo<TPresenter, TModel> CreateTransition<TPresenter, TModel>(TModel model, CancellationToken cancellationToken = default)
            where TPresenter : Screen<TModel> where TModel : ScreenModel
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