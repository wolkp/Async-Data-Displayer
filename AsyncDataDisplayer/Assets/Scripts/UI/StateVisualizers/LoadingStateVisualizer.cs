using UnityEngine;
using Zenject;

public class LoadingStateVisualizer : MonoBehaviour
{
    [Inject] private readonly SignalBus _signalBus;

    [SerializeField] private GameObject _loadingSpinner;

    private LoadingState _loadingState;

    private void OnEnable()
    {
        _signalBus.Subscribe<PageLoadingStateChangedSignal>(OnPageLoadingStatusChanged);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<PageLoadingStateChangedSignal>(OnPageLoadingStatusChanged);
    }

    private void OnPageLoadingStatusChanged(PageLoadingStateChangedSignal signal)
    {
        if (_loadingState != signal.NewState)
        {
            _loadingState = signal.NewState;
            UpdateLoadingState(_loadingState);
        }
    }

    private void UpdateLoadingState(LoadingState loadingState)
    {
        if (_loadingSpinner == null)
        {
            Debug.LogError($"Loading spinner is not assigned in the inspector for {gameObject.name}.");
            return;
        }

        _loadingSpinner.SetActive(loadingState == LoadingState.LoadingInProgress);
    }
}