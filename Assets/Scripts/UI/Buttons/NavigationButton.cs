using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public abstract class NavigationButton : MonoBehaviour
{
    [Inject] protected readonly SignalBus signalBus;

    private Button _button;

    protected abstract bool ShouldBeInteractableInState(NavigationState state);

    protected abstract void OnClick();

    protected virtual void Awake()
    {
        _button = GetComponent<Button>();
    }

    protected virtual void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
        signalBus.Subscribe<NavigationStateChangedSignal>(OnNavigationStateChanged);
    }

    protected virtual void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
        signalBus.Unsubscribe<NavigationStateChangedSignal>(OnNavigationStateChanged);
    }
    private void OnNavigationStateChanged(NavigationStateChangedSignal signal)
    {
        bool shouldBeInteractable = ShouldBeInteractableInState(signal.NewState);
        SetInteractableState(shouldBeInteractable);
    }

    private void SetInteractableState(bool isInteractable)
    {
        _button.interactable = isInteractable;
    }
}