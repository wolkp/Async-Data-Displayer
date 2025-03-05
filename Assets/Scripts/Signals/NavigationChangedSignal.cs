public class NavigationStateChangedSignal : ISignal
{
    public readonly NavigationState NewState;

    public NavigationStateChangedSignal(NavigationState newState)
    {
        NewState = newState;
    }
}

public enum NavigationState
{
    CanNavigateBothWays,
    CanGoNext,
    CanGoPrevious,
    CannotNavigate
}