using System;
using System.Text;
using System.Threading;
using Better.UISystem.Runtime.Common;
using Better.UISystem.Runtime.PopupsSystem.Interfaces;
using Better.UISystem.Runtime.PopupsSystem.Popups;

namespace Better.UISystem.Runtime.PopupsSystem.Transitions
{
    public abstract class OpenPopupTransitionInfo : PopupTransitionInfo
    {
        public Type PresenterType { get; }
        public PopupModel DerivedModel { get; }
        public int Priority { get; private set; }

        private ConditionIterator _readinessConditions;
        private ConditionIterator _cancellationConditions;

        public OpenPopupTransitionInfo(IPopupTransitionRunner runner, Type presenterType, PopupModel model, CancellationToken cancellationToken)
            : base(runner, cancellationToken)
        {
            PresenterType = presenterType;
            DerivedModel = model;
            _readinessConditions = new ConditionIterator();
            _cancellationConditions = new ConditionIterator();
        }

        protected void SetPriority(int value)
        {
            Priority = value;
        }

        #region Conditions

        protected void AddReadinessConditionInternal(Condition condition)
        {
            ValidateMutable();

            _readinessConditions.Add(condition);
        }

        protected void AddCancellationConditionInternal(Condition condition)
        {
            ValidateMutable();

            _cancellationConditions.Add(condition);
        }

        #endregion

        protected override StringBuilder BuildLogInfo()
        {
            var builder = base.BuildLogInfo();
            builder.AppendLine()
                .AppendFormat("{0}:{1}", nameof(PresenterType), PresenterType.Name)
                .AppendLine()
                .AppendFormat("{0}:{1}", nameof(_readinessConditions), _readinessConditions.Count.ToString())
                .AppendLine()
                .AppendFormat("{0}:{1}", nameof(_cancellationConditions), _cancellationConditions.Count.ToString());

            return builder;
        }
    }

    public abstract class OpenPopupTransitionInfo<TPresenter, TModel> : OpenPopupTransitionInfo
        where TPresenter : Popup<TModel>
        where TModel : PopupModel
    {

        protected OpenPopupTransitionInfo(IPopupTransitionRunner runner, PopupModel model, CancellationToken cancellationToken)
            : base(runner, typeof(TPresenter), model, cancellationToken)
        {
        }
    }
}