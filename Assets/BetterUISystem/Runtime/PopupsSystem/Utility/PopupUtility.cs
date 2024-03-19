using System.Threading.Tasks;
using Better.UISystem.Runtime.PopupsSystem.Interfaces;
using Better.UISystem.Runtime.PopupsSystem.Popups;

namespace Better.UISystem.Runtime
{
    public static class PopupUtility
    {
#if BETTER_SERVICES && BETTER_LOCATOR
        private static readonly Locators.Runtime.ServiceProperty<PopupService> _serviceProperty = new();
#endif

        public static IPopupSystem GetSystem()
        {
            return null;
        }

        public static Task<TransitionResult<TPresenter>> ForceOpen<TPresenter, TModel>() where TPresenter : Popup<TModel> where TModel : PopupModel, new()
        {
            var model = new TModel();
            return GetSystem().CreateForceTransition<TPresenter, TModel>(model).RunAsync();
        }

        public static Task<TransitionResult<TPresenter>> ForceOpen<TPresenter, TModel>(TModel model)
            where TPresenter : Popup<TModel>
            where TModel : PopupModel
        {
            return GetSystem().CreateForceTransition<TPresenter, TModel>(model).RunAsync();
        }

        public static Task<TransitionResult<TPresenter>> ScheduleOpen<TPresenter, TModel>(int priority = default) where TPresenter : Popup<TModel> where TModel : PopupModel, new()
        {
            var model = new TModel();
            return GetSystem().CreateScheduleTransition<TPresenter, TModel>(model)
                .OverridePriority(priority)
                .RunAsync();
        }

        public static Task<TransitionResult<TPresenter>> ScheduleOpen<TPresenter, TModel>(TModel model, int priority)
            where TPresenter : Popup<TModel>
            where TModel : PopupModel
        {
            return GetSystem().CreateScheduleTransition<TPresenter, TModel>(model)
                .OverridePriority(priority)
                .RunAsync();
        }
    }
}