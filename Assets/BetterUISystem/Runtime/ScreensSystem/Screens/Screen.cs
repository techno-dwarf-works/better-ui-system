using System.Threading.Tasks;
using Better.Components.Runtime;
using Better.UISystem.Runtime.ScreensSystem.Interfaces;
using Better.Validation.Runtime.Attributes;
using UnityEngine;

namespace Better.UISystem.Runtime.ScreensSystem.Screens
{
    [RequireComponent(typeof(ScreenView))]
    public abstract class Screen<TModel> : UIMonoBehaviour, IScreen
        where TModel : ScreenModel
    {
        private ScreenView _derivedView;
        protected TModel Model { get; private set; }

        protected virtual void Awake()
        {
            _derivedView = GetDerivedView();
        }

        protected virtual ScreenView GetDerivedView()
        {
            return GetComponent<ScreenView>();
        }

        public void SetModel(TModel model)
        {
            Model = model;
            Rebuild();
        }

        #region IScreen

        Task IScreen.InitializeAsync()
        {
            _derivedView.Interactable = false;
            _derivedView.Displayed = false;

            return OnInitializeAsync();
        }

        Task IScreen.PrepareShowAsync()
        {
            return OnPrepareShowAsync();
        }

        async Task IScreen.ShowAsync()
        {
            _derivedView.Interactable = true;
            _derivedView.Displayed = true;
            await _derivedView.ShowAsync();
            await OnShowAsync();
        }

        Task IScreen.PrepareHideAsync()
        {
            _derivedView.Interactable = false;
            return OnPrepareHideAsync();
        }

        async Task IScreen.HideAsync()
        {
            _derivedView.Interactable = false;
            await _derivedView.HideAsync();
            await OnHideAsync();
            _derivedView.Displayed = false;
        }

        #endregion

        protected abstract Task OnInitializeAsync();
        protected abstract Task OnPrepareShowAsync();
        protected abstract Task OnShowAsync();
        protected abstract void Rebuild();
        protected abstract Task OnPrepareHideAsync();
        protected abstract Task OnHideAsync();

        protected virtual void OnDestroy()
        {
        }
    }
    
    public abstract class Screen<TModel, TView> : Screen<TModel>
        where TModel : ScreenModel
        where TView : ScreenView
    {
        [Header("REFERENCES")] [NotNull] [SerializeField]
        private TView _view;

        protected TView View => _view;

        protected override ScreenView GetDerivedView() => View;
    }
}