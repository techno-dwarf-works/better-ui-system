﻿using System.Threading;
using Better.UISystem.Runtime.ScreensSystem.Interfaces;
using Better.UISystem.Runtime.ScreensSystem.Screens;
using Better.UISystem.Runtime.ScreensSystem.Transitions;
using UnityEngine;

namespace Better.UISystem.Runtime.ScreensSystem
{
    public class ScreenSystem : IScreenSystem
    {
        private readonly InternalScreenSystem _internalSystem;
        private RectTransform _rectTransform;

        public ScreenSystem(RectTransform container)
        {
            _internalSystem = new InternalScreenSystem(container);
        }

        #region IScreenSystem
        
        public int HistoryCount => _internalSystem.HistoryCount;

        public RectTransform Container => _internalSystem.Container;

        public ScreenTransitionInfo<TPresenter, TModel> CreateTransition<TPresenter, TModel>(TModel model, CancellationToken cancellationToken = default)
            where TPresenter : Screen<TModel> where TModel : ScreenModel
        {
            return _internalSystem.CreateTransition<TPresenter, TModel>(model, cancellationToken);
        }

        public HistoryTransitionInfo CreateHistoryTransition(int historyDepth = 1, CancellationToken cancellationToken = default)
        {
            return _internalSystem.CreateHistoryTransition(historyDepth, cancellationToken);
        }

        public bool IsOpened<TPresenter, TModel>() where TPresenter : Screen<TModel> where TModel : ScreenModel
        {
            return _internalSystem.IsOpened<TPresenter, TModel>();
        }

        public bool TryGetOpened<TPresenter, TModel>(out TPresenter screen) where TPresenter : Screen<TModel> where TModel : ScreenModel
        {
            return _internalSystem.TryGetOpened<TPresenter, TModel>(out screen);
        }

        #endregion
    }
}