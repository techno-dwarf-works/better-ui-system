using System.Threading.Tasks;
using Better.UISystem.Runtime.Common;
using Better.UISystem.Runtime.PopupsSystem.Popups;
using Better.UISystem.Runtime.PopupsSystem.Transitions;

namespace Better.UISystem.Runtime.PopupsSystem.Interfaces
{
    public interface IPopupTransitionRunner : ITransitionRunner
    {
        public Task CloseRunTransition(ClosePopupTransitionInfo info);

        public Task<TransitionResult<TPresenter>> ScheduleRunTransition<TPresenter, TModel>(SchedulePopupTransitionInfo<TPresenter, TModel> info)
            where TPresenter : Popup<TModel>
            where TModel : PopupModel;

        public Task<TransitionResult<TPresenter>> ForceRunTransition<TPresenter, TModel>(ForcePopupTransitionInfo<TPresenter, TModel> info)
            where TPresenter : Popup<TModel> 
            where TModel : PopupModel;
    }
}