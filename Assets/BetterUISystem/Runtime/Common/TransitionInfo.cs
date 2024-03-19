using System;
using System.Text;
using System.Threading;
using Better.Extensions.Runtime;
using UnityEngine;

namespace Better.UISystem.Runtime.Common
{
    public abstract class TransitionInfo
    {
        private CancellationTokenSource _tokenSource;

        public bool Mutable { get; private set; }

        public CancellationToken CancellationToken => _tokenSource.Token;

        protected TransitionInfo(CancellationToken cancellationToken)
        {
            _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Mutable = true;
        }

        protected bool ValidateMutable(bool logException = true)
        {
            if (!Mutable && logException)
            {
                var message = "Is immutable";
                DebugUtility.LogException<AccessViolationException>(message);
            }

            return Mutable;
        }

        protected bool MakeImmutable()
        {
            if (!Mutable)
            {
                var message = "Already immutable";
                Debug.LogError(message);
                return false;
            }

            Mutable = false;
            return true;
        }

        public virtual bool Cancel()
        {
            Mutable = false;
            _tokenSource.Cancel();
            return true;
        }
        
        public virtual bool IsRelevant()
        {
            return !_tokenSource.IsCancellationRequested;
        }

        public string GetLogInfo()
        {
            var builder = BuildLogInfo();
            return builder.ToString();
        }

        protected virtual StringBuilder BuildLogInfo()
        {
            var builder = new StringBuilder()
                .AppendFormat("{0}:{1}", nameof(Mutable), Mutable.ToString())
                .AppendLine()
                .AppendFormat("{0}:{1}", nameof(IsRelevant), IsRelevant().ToString());

            return builder;
        }
    }
}