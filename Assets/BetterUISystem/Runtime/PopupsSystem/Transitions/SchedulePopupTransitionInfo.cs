using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Better.UISystem.Runtime.Common;
using Better.UISystem.Runtime.PopupsSystem.Interfaces;
using Better.UISystem.Runtime.PopupsSystem.Popups;

namespace Better.UISystem.Runtime.PopupsSystem.Transitions
{
    public class SchedulePopupTransitionInfo<TPresenter, TModel> : OpenPopupTransitionInfo<TPresenter, TModel> 
        where TPresenter : Popup<TModel> 
        where TModel : PopupModel
    {

        public SchedulePopupTransitionInfo(IPopupTransitionRunner runner, PopupModel model, CancellationToken cancellationToken)
            : base(runner, model, cancellationToken)
        {
        }

        public SchedulePopupTransitionInfo<TPresenter, TModel> OverridePriority(int value)
        {
            SetPriority(value);
            return this;
        }
        
        public SchedulePopupTransitionInfo<TPresenter, TModel> AddReadinessCondition(Condition condition)
        {
            AddReadinessConditionInternal(condition);
            return this;
        }

        public SchedulePopupTransitionInfo<TPresenter, TModel> AddCancellationCondition(Condition condition)
        {
            AddCancellationConditionInternal(condition);
            return this;
        }
        
        public Task<TransitionResult<TPresenter>> RunAsync()
        {
            ValidateRun();
            return Runner.ScheduleRunTransition(this);
        }
    }
}