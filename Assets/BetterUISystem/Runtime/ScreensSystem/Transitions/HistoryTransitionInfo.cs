using System.Threading;
using System.Threading.Tasks;
using Better.Extensions.Runtime;
using Better.UISystem.Runtime.ScreensSystem.Interfaces;

namespace Better.UISystem.Runtime.ScreensSystem.Transitions
{
    public class HistoryTransitionInfo : ScreenTransitionInfo
    {
        public int HistoryDepth { get; private set; }
        public bool AllowExceptions { get; private set; }
        public bool UseSafeDepth { get; private set; }

        public HistoryTransitionInfo(IScreenTransitionRunner runner, int historyDepth, CancellationToken cancellationToken) : base(runner, cancellationToken)
        {
            HistoryDepth = historyDepth;
            AllowExceptions = true;
            UseSafeDepth = false;
        }

        public Task RunAsync()
        {
            ValidateMutable();
            MakeImmutable();

            return Runner.RunAsync(this);
        }

        public HistoryTransitionInfo SuppressExceptions()
        {
            AllowExceptions = false;
            return this;
        }
        
        public HistoryTransitionInfo MakeUseSafeDepth()
        {
            UseSafeDepth = true;
            return this;
        }

        public void Run()
        {
            RunAsync().Forget();
        }
    }
}