#if BETTER_SERVICES
using System.Threading;
using System.Threading.Tasks;
using Better.Services.Runtime;
using Better.UISystem.Runtime.PopupsSystem.Interfaces;
using Better.UISystem.Runtime.PopupsSystem.Popups;
using Better.UISystem.Runtime.PopupsSystem.Transitions;
using UnityEngine;

namespace Better.UISystem.Runtime.PopupsSystem
{
    [RequireComponent(typeof(RectTransform))]
    public class ScreenService : MonoService, IPopupSystem
    {
        private IPopupSystem _internalSystem;

        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            var rectTransform = GetComponent<RectTransform>();
            _internalSystem = new InternalPopupSystem(rectTransform);
            return Task.CompletedTask;
        }

        protected override Task OnPostInitializeAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
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
#endif