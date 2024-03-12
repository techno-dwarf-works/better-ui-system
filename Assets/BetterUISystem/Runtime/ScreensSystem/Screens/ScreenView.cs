using System.Threading;
using System.Threading.Tasks;
using Better.Components.Runtime;
using UnityEngine;

namespace Better.UISystem.Runtime.ScreensSystem.Screens
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class ScreenView : UIMonoBehaviour
    {
        //   [NotNull] [SerializeReference] protected IAnimation _animation;

        private CanvasGroup _canvasGroup;

        protected CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                {
                    _canvasGroup = GetComponent<CanvasGroup>();
                }

                return _canvasGroup;
            }
        }

        public bool Interactable
        {
            get => CanvasGroup.interactable;
            set => CanvasGroup.interactable = value;
        }

        public bool Displayed
        {
            get => CanvasGroup.alpha > 0f;
            set => CanvasGroup.alpha = value ? 1f : 0f;
        }

        protected virtual void Awake()
        {
        }

        public virtual Task ShowAsync()
        {
            //TODO: Pass cancellationToken from service
            return Task.CompletedTask;
        }

        public virtual Task HideAsync()
        {
            return Task.CompletedTask;
        }
    }
}