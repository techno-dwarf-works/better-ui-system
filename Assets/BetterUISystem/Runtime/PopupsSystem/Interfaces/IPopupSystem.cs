using System.Threading;
using System.Threading.Tasks;
using Better.UISystem.Runtime.PopupsSystem.Popups;
using Better.UISystem.Runtime.PopupsSystem.Transitions;
using UnityEngine;

namespace Better.UISystem.Runtime.PopupsSystem.Interfaces
{
    public interface IPopupSystem<TDerived, TDerivedPopup, TDerivedModel>
        where TDerived : IPopup
        where TDerivedPopup : Popup, TDerived
        where TDerivedModel : PopupModel
    {
        RectTransform Container { get; }
        bool HasOpened { get; }
        bool InTransition { get; }

        
        public ForcePopupTransitionInfo<TPresenter, TModel> CreateForceTransition<TPresenter, TModel>(TModel model,
            CancellationToken cancellationToken = default)
            where TPresenter : Popup<TModel>, TDerived
            where TModel : TDerivedModel;

        public SchedulePopupTransitionInfo<TPresenter, TModel> CreateScheduleTransition<TPresenter, TModel>(TModel model,
            CancellationToken cancellationToken = default)
            where TPresenter : Popup<TModel>, TDerived
            where TModel : TDerivedModel;

        public Task ClosePopup(TDerivedPopup popup);
    }
}