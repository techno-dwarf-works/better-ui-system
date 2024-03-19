using System.Threading;
using System.Threading.Tasks;
using Better.UISystem.Runtime.PopupsSystem.Interfaces;

namespace Better.UISystem.Runtime.PopupsSystem.Transitions
{
    public class ClosePopupTransitionInfo : PopupTransitionInfo
    {
        public ClosePopupTransitionInfo(IPopupTransitionRunner runner, CancellationToken cancellationToken)
            : base(runner, cancellationToken)
        {
        }

        public Task Run()
        {
            ValidateRun();
            return Runner.CloseRunTransition(this);
        }
    }
}