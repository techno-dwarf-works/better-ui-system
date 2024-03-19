using System;
using System.Threading;
using System.Threading.Tasks;
using Better.UISystem.Runtime.PopupsSystem.Interfaces;
using Better.UISystem.Runtime.PopupsSystem.Popups;

namespace Better.UISystem.Runtime.PopupsSystem.Transitions
{
    public class ForcePopupTransitionInfo<TPresenter, TModel> : OpenPopupTransitionInfo<TPresenter, TModel> 
        where TPresenter : Popup<TModel> 
        where TModel : PopupModel
    {
        public ForcePopupTransitionInfo(IPopupTransitionRunner runner, PopupModel model, CancellationToken cancellationToken)
            : base(runner, model, cancellationToken)
        {
        }
        
        public Task<TransitionResult<TPresenter>> RunAsync()
        {
            ValidateRun();
            return Runner.ForceRunTransition(this);
        }
    }
}