using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Better.Extensions.Runtime;
using Better.UISystem.Runtime.PopupsSystem.Interfaces;
using Better.UISystem.Runtime.PopupsSystem.Popups;
using Better.UISystem.Runtime.PopupsSystem.Transitions;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Better.UISystem.Runtime
{
    [Serializable]
    public abstract class InternalPopupSystem<TDerived, TDerivedPopup, TDerivedModel> : IPopupTransitionRunner, IPopupSystem<TDerived, TDerivedPopup, TDerivedModel>
        where TDerived : IPopup
        where TDerivedPopup : Popup, TDerived
        where TDerivedModel : PopupModel
    {
        private Dictionary<Type, TDerivedPopup> _popupPrefabMap;
        private TDerived _openedPopup;
        private PopupTransitionInfo _currentTransition;
        private TaskCompletionSource<TDerived> _transitionSource;
        private SortedList<int, OpenPopupTransitionInfo> _scheduledTransitions;

        public RectTransform Container { get; private set; }
        public bool HasOpened => _openedPopup != null;
        public bool InTransition => _currentTransition != null;

        public InternalPopupSystem(RectTransform container)
        {
            Container = container;
            _scheduledTransitions = new();
            PlayerLoopUtility.SubscribeToLoop<PostLateUpdate>(LateUpdate);
            CreatePopupPrefabMap();
        }

        ~InternalPopupSystem()
        {
            PlayerLoopUtility.UnsubscribeFromLoop<PostLateUpdate>(LateUpdate);
        }

        protected abstract void OnScheduleAdded<TTransitionInfo>(TTransitionInfo info) where TTransitionInfo : PopupTransitionInfo;

        protected abstract void OnScheduleRemoved<TTransitionInfo>(TTransitionInfo info) where TTransitionInfo : PopupTransitionInfo;

        protected abstract void OnOpened(TDerived openedPopup);

        protected abstract TDerivedPopup[] GetPrefabs();

        private void CreatePopupPrefabMap()
        {
            _popupPrefabMap = new();

            foreach (var popup in GetPrefabs())
            {
                if (EqualityComparer<TDerivedPopup>.Default.Equals(popup, null))
                {
                    var message = $"Prefab is null";
                    Debug.LogWarning(message);
                }

                var key = popup.GetType();
                _popupPrefabMap.TryAdd(key, popup);
            }
        }

        private void LateUpdate()
        {
            UpdateScheduledTransitions();
            TryNextPopup();
        }

        private void UpdateScheduledTransitions()
        {
            for (int i = _scheduledTransitions.Count - 1; i >= 0; i--)
            {
                var popupTransitionInfo = _scheduledTransitions[i];
                if (popupTransitionInfo.IsRelevant()) continue;

                popupTransitionInfo.Cancel();
                _scheduledTransitions.RemoveAt(i);
                OnScheduleRemoved(popupTransitionInfo);
            }
        }

        public ForcePopupTransitionInfo<TPresenter, TModel> CreateForceTransition<TPresenter, TModel>(TModel model,
            CancellationToken cancellationToken = default)
            where TPresenter : Popup<TModel>, TDerived
            where TModel : TDerivedModel
        {
            var transition = new ForcePopupTransitionInfo<TPresenter, TModel>(this, model, cancellationToken);

            return transition;
        }

        public SchedulePopupTransitionInfo<TPresenter, TModel> CreateScheduleTransition<TPresenter, TModel>(TModel model,
            CancellationToken cancellationToken = default)
            where TPresenter : Popup<TModel>, TDerived
            where TModel : TDerivedModel
        {
            var transition = new SchedulePopupTransitionInfo<TPresenter, TModel>(this, model, cancellationToken);

            return transition;
        }

        public Task ClosePopup(TDerivedPopup popup)
        {
            if (EqualityComparer<TDerived>.Default.Equals(_openedPopup, popup))
            {
                var message = $"Unexpected {nameof(popup)})";
                Debug.LogWarning(message);
                return Task.CompletedTask;
            }

            UpdateScheduledTransitions();
            if (TryPopScheduledTransition(out var scheduledTransition))
            {
                return RunTransitionAsync(scheduledTransition);
            }

            var transitionInfo = new ClosePopupTransitionInfo(this, CancellationToken.None);
            return transitionInfo.Run();
        }

        private void TryNextPopup()
        {
            if (HasOpened || InTransition) return;

            if (TryPopScheduledTransition(out var poppedTransitionInfo))
            {
                RunTransitionAsync(poppedTransitionInfo).Forget();
            }
        }

        #region PopupTransitionRunner

        Task IPopupTransitionRunner.CloseRunTransition(ClosePopupTransitionInfo info)
        {
            return RunTransitionAsync(info);
        }

        Task<TransitionResult<TPresenter>> IPopupTransitionRunner.ScheduleRunTransition<TPresenter, TModel>(
            SchedulePopupTransitionInfo<TPresenter, TModel> info)
        {
            _scheduledTransitions.Add(info.Priority, info);

            OnScheduleAdded(info);

            UpdateScheduledTransitions();
            TryNextPopup();

            return AwaitTransitionResult(info);
        }

        async Task<TransitionResult<TPresenter>> IPopupTransitionRunner.ForceRunTransition<TPresenter, TModel>(
            ForcePopupTransitionInfo<TPresenter, TModel> info)
        {
            RunTransitionAsync(info).Forget();

            return await AwaitTransitionResult(info);
        }

        private async Task<TransitionResult<TPresenter>> AwaitTransitionResult<TPresenter, TModel>(OpenPopupTransitionInfo<TPresenter, TModel> info)
            where TPresenter : Popup<TModel>
            where TModel : PopupModel
        {
            await TaskUtility.WaitUntil(() => _currentTransition == info, info.CancellationToken);
            await TaskUtility.WaitWhile(() => InTransition, info.CancellationToken);
            if (_openedPopup is TPresenter presenter)
            {
                return new TransitionResult<TPresenter>(true, presenter);
            }

            return new TransitionResult<TPresenter>(false, default);
        }

        #endregion

        private async Task RunTransitionAsync(PopupTransitionInfo info)
        {
            if (info.Mutable)
            {
                var message = $"{info} cannot be mutable";
                Debug.LogError(message);
                info.Cancel();
                return;
            }

            await TaskUtility.WaitUntil(() => !InTransition);
            if (!info.IsRelevant())
            {
                info.Cancel();
                return;
            }

            _currentTransition = info;
            var inMessage = $"In, {info.GetLogInfo()}";
            Debug.Log(inMessage);

            TDerived directedPopup = default(TDerived);
            if (info is OpenPopupTransitionInfo directedTransitionInfo)
            {
                var directedPresenter = await InstantiatePopupAsync(directedTransitionInfo.PresenterType);
                directedPopup = directedPresenter;
                await directedPopup.InitializeAsync();
                directedPresenter.SetModel(directedTransitionInfo.DerivedModel);
            }

            if (_openedPopup == null && directedPopup == null)
            {
                var unexpectedDirectedMessage = $"Unexpected {nameof(directedTransitionInfo)}";
                Debug.LogWarning(unexpectedDirectedMessage);
                _currentTransition = null;
                info.Cancel();
                return;
            }

            var rootPopup = _openedPopup;
            _openedPopup = directedPopup;

            var sequence = GetTransitionSequence(info);

            await sequence.DoPlay(rootPopup, _openedPopup);
            rootPopup?.RectTransform.DestroyGameObject();

            if (_currentTransition != info)
            {
                var unexpectedOutMessage = $"Unexpected pre-out state";
                DebugUtility.LogException<InvalidOperationException>(unexpectedOutMessage);
                info.Cancel();
                return;
            }

            OnOpened(_openedPopup);

            _currentTransition = null;
            var outMessage = $"Out, {info.GetLogInfo()}";
            Debug.Log(outMessage);
        }

        protected abstract PopupsSequence GetTransitionSequence(PopupTransitionInfo info);

        private bool TryPopScheduledTransition(out PopupTransitionInfo transitionInfo)
        {
            for (var index = _scheduledTransitions.Count - 1; index >= 0; index--)
            {
                var scheduledTransition = _scheduledTransitions[index];
                if (!scheduledTransition.IsReadiness()) continue;

                transitionInfo = scheduledTransition;
                return true;
            }

            transitionInfo = null;
            return false;
        }

        // TODO: Upgrade to AssetService
        private async Task<TDerivedPopup> InstantiatePopupAsync(Type presenterType)
        {
            if (!_popupPrefabMap.TryGetValue(presenterType, out var prefab))
            {
                var message = $"Unexpected {nameof(presenterType)}({presenterType})";
                throw new InvalidOperationException(message);
            }

#if UNITY_EDITOR
            //TODO: Controllable unexpected operation
            var framesDelay = Random.Range(1, 20);
            await TaskUtility.WaitFrame(framesDelay);
#endif

            var popup = Object.Instantiate(prefab, Container);
            return popup;
        }
    }
}