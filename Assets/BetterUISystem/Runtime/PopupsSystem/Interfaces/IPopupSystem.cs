using System.Threading;
using System.Threading.Tasks;
using Better.UISystem.Runtime.PopupsSystem.Popups;
using Better.UISystem.Runtime.PopupsSystem.Transitions;
using UnityEngine;

namespace Better.UISystem.Runtime.PopupsSystem.Interfaces
{
    public interface IPopupSystem<TDerived, TDerivedPopup> 
        where TDerived : IPopup
        where TDerivedPopup : Popup, TDerived
    {
        RectTransform Container { get; }
        bool HasOpened { get; }
        bool InTransition { get; }

        public ForcePopupTransitionInfo<TPresenter, TModel> CreateForceTransition<TPresenter, TModel>(TModel model, CancellationToken cancellationToken = default)
            where TPresenter : Popup<TModel>
            where TModel : PopupModel;

        public SchedulePopupTransitionInfo<TPresenter, TModel> CreateScheduleTransition<TPresenter, TModel>(TModel model, CancellationToken cancellationToken = default)
            where TPresenter : Popup<TModel>
            where TModel : PopupModel;

        public Task ClosePopup(TDerivedPopup popup);
    }
}