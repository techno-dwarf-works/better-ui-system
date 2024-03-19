using System;
using System.Threading;
using System.Threading.Tasks;
using Better.Extensions.Runtime;
using Better.UISystem.Runtime.Common;
using Better.UISystem.Runtime.ScreensSystem.Interfaces;
using Better.UISystem.Runtime.ScreensSystem.Screens;
using Better.UISystem.Runtime.ScreensSystem.Sequences;

namespace Better.UISystem.Runtime.ScreensSystem.Transitions
{
    public abstract class ScreenTransitionInfo : TransitionInfo
    {
        private readonly IScreenTransitionRunner _runner;

        protected IScreenTransitionRunner Runner => _runner;

        public Type SequenceType { get; private set; }
        public bool OverridenSequence { get; private set; }
        
        public ScreenTransitionInfo(IScreenTransitionRunner runner, CancellationToken cancellationToken)
            : base(cancellationToken)
        {
            _runner = runner;
        }

        protected void OverrideSequence<TSequence>() where TSequence : ScreensSequence
        {
            if (!ValidateMutable())
            {
                return;
            }

            OverridenSequence = true;
            SequenceType = typeof(TSequence);
        }
    }
    
    public class ScreenTransitionInfo<TPresenter, TModel> : ScreenTransitionInfo
        where TPresenter : Screen<TModel>
        where TModel : ScreenModel
    {
        public TModel Model { get; }

        public bool AllowMultiInstance { get; private set; }

        public ScreenTransitionInfo(IScreenTransitionRunner runner, TModel model, CancellationToken cancellationToken)
            : base(runner, cancellationToken)
        {
            Model = model;
            AllowMultiInstance = false;
        }
        
        public ScreenTransitionInfo Sequence<TSequence>()
            where TSequence : ScreensSequence
        {
            OverrideSequence<TSequence>();
            return this;
        }

        public ScreenTransitionInfo AllowMultiInstancing()
        {
            if (ValidateMutable())
            {
                AllowMultiInstance = true;
            }

            return this;
        }

        public Task<TPresenter> RunAsync()
        {
            ValidateMutable();
            MakeImmutable();

            return Runner.RunAsync(this);
        }

        public void Run()
        {
            RunAsync().Forget();
        }
    }
}