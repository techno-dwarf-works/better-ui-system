using System;
using System.Threading;
using Better.UISystem.Runtime.Common;
using Better.UISystem.Runtime.PopupsSystem.Interfaces;

namespace Better.UISystem.Runtime.PopupsSystem.Transitions
{
    public abstract class PopupTransitionInfo : TransitionInfo
    {
        protected IPopupTransitionRunner Runner { get; }
        public Type SequenceType { get; private set; }
        public bool OverridenSequence { get; private set; }

        public PopupTransitionInfo(IPopupTransitionRunner runner, CancellationToken cancellationToken)
            : base(cancellationToken)
        {
            Runner = runner;
        }

        protected void OverrideSequence<TSequence>() where TSequence : PopupsSequence
        {
            if (!ValidateMutable())
            {
                return;
            }

            OverridenSequence = true;
            SequenceType = typeof(TSequence);
        }
        
        public virtual bool IsReadiness()
        {
            return IsRelevant();
        }

        protected void ValidateRun()
        {
            ValidateMutable();
            MakeImmutable();
        }
    }
}