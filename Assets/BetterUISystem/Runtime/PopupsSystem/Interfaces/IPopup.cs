using System.Threading.Tasks;
using UnityEngine;

namespace Better.UISystem.Runtime.PopupsSystem.Interfaces
{
    public interface IPopup
    {
        public RectTransform RectTransform { get; }
        public Task InitializeAsync();
        public Task PrepareShowAsync();
        public Task ShowAsync();
        public Task PrepareHideAsync();
        public Task HideAsync();
    }
}