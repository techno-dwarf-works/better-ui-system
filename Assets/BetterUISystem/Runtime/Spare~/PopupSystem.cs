using System;
using System.Threading;
using System.Threading.Tasks;
using Better.UISystem.Runtime.PopupsSystem.Interfaces;
using Better.UISystem.Runtime.PopupsSystem.Popups;
using Better.UISystem.Runtime.PopupsSystem.Transitions;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Better.UISystem.Runtime
{
    public class PopupSystem : IPopupSystem
    {
        private readonly InternalPopupSystem _internalSystem;
        private RectTransform _rectTransform;

        public PopupSystem()
        {
            var container = FindOrCreateCanvas();
            _internalSystem = new InternalPopupSystem(container);
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

            var systemGameObject = new GameObject(nameof(PopupSystem), canvasComponents);
            return systemGameObject.GetComponent<RectTransform>();
        }

        #region IPopupSystem

        public RectTransform Container => _internalSystem.Container;

        public bool HasOpened => _internalSystem.HasOpened;

        public bool InTransition => _internalSystem.InTransition;

        public bool IsOpened<TPresenter>() where TPresenter : Popup
        {
            return _internalSystem.IsOpened<TPresenter>();
        }

        public bool TryGetOpened<TPresenter>(out TPresenter popup) where TPresenter : Popup
        {
            return _internalSystem.TryGetOpened(out popup);
        }

        public ForcePopupTransitionInfo<TPresenter, TModel> CreateForceTransition<TPresenter, TModel>(TModel model,
            CancellationToken cancellationToken = default) 
            where TPresenter : Popup<TModel> 
            where TModel : PopupModel
        {
            return _internalSystem.CreateForceTransition<TPresenter, TModel>(model, cancellationToken);
        }

        public SchedulePopupTransitionInfo<TPresenter, TModel> CreateScheduleTransition<TPresenter, TModel>(TModel model,
            CancellationToken cancellationToken = default) 
            where TPresenter : Popup<TModel> 
            where TModel : PopupModel
        {
            return _internalSystem.CreateScheduleTransition<TPresenter, TModel>(model, cancellationToken);
        }

        public Task ClosePopup(IPopup popup)
        {
            return _internalSystem.ClosePopup(popup);
        }

        #endregion
    }
}