using System.Threading.Tasks;
using Better.Components.Runtime;
using Better.Extensions.Runtime;
using Better.UISystem.Runtime.PopupsSystem.Interfaces;
using Better.Validation.Runtime.Attributes;
using UnityEngine;

namespace Better.UISystem.Runtime.PopupsSystem.Popups
{
    [RequireComponent(typeof(PopupView))]
    public abstract class Popup : UIMonoBehaviour, IPopup
    {
        private PopupView _derivedView;
        protected PopupModel DerivedModel { get; private set; }

        protected virtual void Awake()
        {
            _derivedView = GetDerivedView();
        }

        protected virtual PopupView GetDerivedView()
        {
            return GetComponent<PopupView>();
        }

        public virtual void SetModel(PopupModel model)
        {
            DerivedModel = model;
            Rebuild();
        }

        protected abstract void Close();

        #region IPopup

        Task IPopup.InitializeAsync()
        {
            _derivedView.Interactable = false;
            _derivedView.Displayed = false;

            return OnInitializeAsync();
        }

        Task IPopup.PrepareShowAsync()
        {
            return OnPrepareShowAsync();
        }

        async Task IPopup.ShowAsync()
        {
            _derivedView.Interactable = true;
            _derivedView.Displayed = true;
            await _derivedView.ShowAsync();
            await OnShowAsync();
        }

        Task IPopup.PrepareHideAsync()
        {
            _derivedView.Interactable = false;
            return OnPrepareHideAsync();
        }

        async Task IPopup.HideAsync()
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
    }

    public abstract class Popup<TModel> : Popup
        where TModel : PopupModel
    {
        protected TModel Model { get; private set; }

        public override void SetModel(PopupModel model)
        {
            if (model is TModel castedModel)
            {
                Model = castedModel;
                base.SetModel(model);
                return;
            }

            var message = $"[{GetType().Namespace}] {nameof(SetModel)}: unexpected {nameof(model)} type({model.GetType()})";
            Debug.LogError(message);
        }
    }

    public abstract class Popup<TModel, TView> : Popup<TModel>
        where TModel : PopupModel
        where TView : PopupView
    {
        [Header("REFERENCES")] [NotNull] [SerializeField]
        private TView _view;

        protected TView View => _view;

        protected override PopupView GetDerivedView() => View;
    }
}