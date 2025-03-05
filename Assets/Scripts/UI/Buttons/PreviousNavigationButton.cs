public class PreviousNavigationButton : NavigationButton
{
    protected override void OnClick()
    {
        signalBus.Fire(new PreviousPageClickedSignal());
    }

    protected override bool ShouldBeInteractableInState(NavigationState state)
    {
        return state == NavigationState.CanNavigateBothWays || state == NavigationState.CanGoPrevious;
    }
}