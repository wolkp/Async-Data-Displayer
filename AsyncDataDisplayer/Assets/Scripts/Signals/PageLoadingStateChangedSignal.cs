public class PageLoadingStateChangedSignal : ISignal
{
    public readonly LoadingState NewState;

    public PageLoadingStateChangedSignal(LoadingState newState)
    {
        NewState = newState;
    }
}

public enum LoadingState
{
    LoadingFinished,
    LoadingInProgress
}