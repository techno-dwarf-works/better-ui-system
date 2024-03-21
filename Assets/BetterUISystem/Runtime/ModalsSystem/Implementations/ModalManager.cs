﻿#if BETTER_SINGLETONS
using System.Threading;
using System.Threading.Tasks;
using Better.Singletons.Runtime;
using Better.UISystem.Runtime.PopupsSystem.Popups;
using Better.UISystem.Runtime.PopupsSystem.Transitions;
using UnityEngine;

namespace Better.UISystem.Runtime
{
    [RequireComponent(typeof(RectTransform))]
    public class ModalManager : MonoSingleton<ModalManager>, IModalSystem
    {
        private IModalSystem _internalSystem;

        private void Awake()
        {
            var rectTransform = GetComponent<RectTransform>();
            _internalSystem = new InternalModalSystem(rectTransform);
        }

        #region IModalSystem

        public RectTransform Container => _internalSystem.Container;

        public bool HasOpened => _internalSystem.HasOpened;

        public bool InTransition => _internalSystem.InTransition;

        public ForcePopupTransitionInfo<TPresenter, TModel> CreateForceTransition<TPresenter, TModel>(TModel model,
            CancellationToken cancellationToken = default)
            where TPresenter : Popup<TModel>, IModal
            where TModel : ModalModel
        {
            return _internalSystem.CreateForceTransition<TPresenter, TModel>(model, cancellationToken);
        }

        public SchedulePopupTransitionInfo<TPresenter, TModel> CreateScheduleTransition<TPresenter, TModel>(TModel model,
            CancellationToken cancellationToken = default)
            where TPresenter : Popup<TModel>, IModal
            where TModel : ModalModel
        {
            return _internalSystem.CreateScheduleTransition<TPresenter, TModel>(model, cancellationToken);
        }

        public Task ClosePopup(Modal popup)
        {
            return _internalSystem.ClosePopup(popup);
        }

        #endregion
    }
}
#endif