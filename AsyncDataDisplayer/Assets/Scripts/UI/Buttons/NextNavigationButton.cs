public class NextNavigationButton : NavigationButton
{
    protected override void OnClick()
    {
        signalBus.Fire(new NextPageClickedSignal());
    }

    protected override bool ShouldBeInteractableInState(NavigationState state)
    {
        return state == NavigationState.CanNavigateBothWays || state == NavigationState.CanGoNext;
    }
}