namespace Better.UISystem.Runtime
{
    public class TransitionResult<TPresenter>
    {
        public bool IsSuccessful { get; }
        public TPresenter Presenter { get; }

        public TransitionResult(bool isSuccessful, TPresenter presenter)
        {
            IsSuccessful = isSuccessful;
            Presenter = presenter;
        }
    }
}