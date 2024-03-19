using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Better.Extensions.Runtime;
using Better.UISystem.Runtime.ScreensSystem.Helpers;
using Better.UISystem.Runtime.ScreensSystem.Interfaces;
using Better.UISystem.Runtime.ScreensSystem.Screens;
using Better.UISystem.Runtime.ScreensSystem.Sequences;
using Better.UISystem.Runtime.ScreensSystem.Transitions;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Better.UISystem.Runtime.ScreensSystem
{
    internal class InternalScreenSystem : IScreenSystem, IScreenTransitionRunner
    {
        private Dictionary<Type, IScreen> _screenPrefabMap;
        private IScreen _openedScreen;
        private readonly Queue<ScreenTransitionInfo> _transitionsQueue;
        private readonly ScreenSystemSettings _settings;
        private readonly Stack<HistoryInfo> _historyStack;

        public int HistoryCount => _historyStack.Count;

        public RectTransform Container { get; }

        public InternalScreenSystem(RectTransform container)
        {
            Container = container;
            _transitionsQueue = new Queue<ScreenTransitionInfo>();
            _historyStack = new Stack<HistoryInfo>();
            _settings = ScreenSystemSettings.Instance;
            CreateScreenPrefabMap();
        }

        private void CreateScreenPrefabMap()
        {
            _screenPrefabMap = new();

            foreach (var screenPrefab in _settings.ScreenPrefabs)
            {
                if (screenPrefab is IScreen screen)
                {
                    var key = screen.GetType();
                    _screenPrefabMap.TryAdd(key, screen);
                }
                else if (screenPrefab == null)
                {
                    var message = $"Prefab is null";
                    Debug.LogWarning(message);
                }
                else
                {
                    var message = $"Unexpected prefab (name:{screenPrefab.name})";
                    Debug.LogWarning(message);
                }
            }
        }

        public ScreenTransitionInfo<TPresenter, TModel> CreateTransition<TPresenter, TModel>(TModel model, CancellationToken cancellationToken = default)
            where TPresenter : Screen<TModel>
            where TModel : ScreenModel
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var transition = new ScreenTransitionInfo<TPresenter, TModel>(this, model, cancellationToken);
            return transition;
        }

        public HistoryTransitionInfo CreateHistoryTransition(int historyDepth = 1, CancellationToken cancellationToken = default)
        {
            if (historyDepth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(historyDepth));
            }

            var transition = new HistoryTransitionInfo(this, historyDepth, cancellationToken);
            return transition;
        }

        public bool IsOpened<TPresenter, TModel>()
            where TPresenter : Screen<TModel>
            where TModel : ScreenModel
        {
            return _openedScreen is TPresenter;
        }

        public bool TryGetOpened<TPresenter, TModel>(out TPresenter screen)
            where TPresenter : Screen<TModel>
            where TModel : ScreenModel
        {
            if (_openedScreen is TPresenter castedScreen)
            {
                screen = castedScreen;
                return true;
            }

            screen = default;
            return false;
        }

        #region ScreenRunAsync

        async Task<TPresenter> IScreenTransitionRunner.RunAsync<TPresenter, TModel>(ScreenTransitionInfo<TPresenter, TModel> info)
        {
            var infoMessage = $"Screen of {typeof(TPresenter).Name}";
            Debug.Log(infoMessage);

            await AwaitTransitionActualization(info);

            var rootScreen = _openedScreen;
            var hasOpenedScreen = rootScreen != null;

            if (hasOpenedScreen && !info.AllowMultiInstance && _openedScreen.GetType() == typeof(TPresenter))
            {
                var presenter = (TPresenter)_openedScreen;
                presenter.SetModel(info.Model);
                OnRunExit(info);
                return presenter;
            }

            var openedPresenter = await InstantiateScreenAsync<TPresenter, TModel>();
            _openedScreen = openedPresenter;

            await _openedScreen.InitializeAsync();
            openedPresenter.SetModel(info.Model);

            var sequence = GetTransitionSequence(info);

            var willCleanStack = _openedScreen is not IStackingProvider;
            if (!hasOpenedScreen)
            {
                await sequence.DoPlay(_openedScreen);
            }
            else
            {
                await sequence.DoPlay(rootScreen, _openedScreen);
                if (!willCleanStack && rootScreen is IStackable stackable)
                {
                    await stackable.PushStackAsync();
                    _historyStack.Push(new HistoryInfo(stackable, info));
                }
                else
                {
                    rootScreen.RectTransform.DestroyGameObject();
                }
            }

            if (willCleanStack)
            {
                ClearStack();
            }

            OnRunExit(info);

            return openedPresenter;
        }


        // TODO: Upgrade to AssetService
        private async Task<TPresenter> InstantiateScreenAsync<TPresenter, TModel>()
            where TPresenter : Screen<TModel>
            where TModel : ScreenModel
        {
            var presenterType = typeof(TPresenter);
            if (!_screenPrefabMap.TryGetValue(presenterType, out var derivedPrefab)
                || derivedPrefab is not TPresenter presenterPrefab)
            {
                var message = $"Unexpected {nameof(presenterType)}({presenterType})";
                throw new InvalidOperationException(message);
            }

#if UNITY_EDITOR
            //TODO: Controllable unexpected operation
            var framesDelay = Random.Range(1, 20);
            await TaskUtility.WaitFrame(framesDelay);
#endif

            var presenter = Object.Instantiate(presenterPrefab, Container);
            return presenter;
        }

        private void ClearStack()
        {
            foreach (var info in _historyStack)
            {
                info.Screen.RectTransform.DestroyGameObject();
            }

            _historyStack.Clear();
        }

        #endregion

        #region HistoryRunAsync

        async Task IScreenTransitionRunner.RunAsync(HistoryTransitionInfo info)
        {
            var infoMessage = $"History screen of {info.HistoryDepth} {nameof(info.HistoryDepth)}";
            Debug.Log(infoMessage);
            
            await AwaitTransitionActualization(info);

            var depth = ValidateDepth(info);

            var historyInfos = new List<HistoryInfo>(depth);
            for (var index = 0; index < depth - 1; index++)
            {
                var bufferInfo = _historyStack.Pop();
                historyInfos.Add(bufferInfo);
            }

            var historyInfo = _historyStack.Pop();

            foreach (var (screen, screenTransitionInfo) in historyInfos)
            {
                await screen.ReleasedFormStackAsync();
                screen.RectTransform.DestroyGameObject();
            }

            var sequence = GetTransitionSequence(info);
            sequence = sequence.GetInverseSequence();

            var rootScreen = _openedScreen;

            var bufferStackable = historyInfo.Screen;
            _openedScreen = bufferStackable;

            await bufferStackable.PopStackAsync();

            if (rootScreen == null)
            {
                await sequence.DoPlay(_openedScreen);
            }
            else
            {
                await sequence.DoPlay(rootScreen, _openedScreen);
                rootScreen.RectTransform.DestroyGameObject();
            }

            OnRunExit(info);
        }

        private int ValidateDepth(HistoryTransitionInfo info)
        {
            var depth = info.HistoryDepth;
            if (depth <= _historyStack.Count)
            {
                return depth;
            }

            if (info.UseSafeDepth)
            {
                return _historyStack.Count;
            }

            if (!info.AllowExceptions)
            {
                return depth;
            }

            var exceptionMessage = $"{nameof(depth)} is bigger than history stack";
            OnRunExit(info);
            throw new IndexOutOfRangeException(exceptionMessage);
        }

        #endregion

        #region Misc

        private async Task AwaitTransitionActualization(ScreenTransitionInfo info)
        {
            _transitionsQueue.Enqueue(info);
            await TaskUtility.WaitUntil(() => _transitionsQueue.Peek().Equals(info));
        }

        private void OnRunExit(ScreenTransitionInfo info)
        {
            if (!_transitionsQueue.Dequeue().Equals(info))
            {
                var unexpectedDequeMessage = "Unexpected dequeue";
                throw new InvalidOperationException(unexpectedDequeMessage);
            }
        }

        private ScreensSequence GetTransitionSequence(ScreenTransitionInfo info)
        {
            if (!info.OverridenSequence || !_settings.TryGetOverridenSequence(info.SequenceType, out var sequence))
            {
                sequence = _settings.GetDefaultSequence();
            }

            return sequence;
        }

        #endregion
    }
}