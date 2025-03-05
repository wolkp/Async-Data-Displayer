using Zenject;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Collections.ObjectModel;

public class PanelController : IPanelController, IInitializable, IDisposable
{
    private readonly IDataServer _dataServer;
    private readonly SignalBus _signalBus;
    private readonly PanelConfig _config;

    private int ItemsPerPage => _config.ItemsPerPage;

    private Dictionary<int, IList<DataItem>> _pageCache = new Dictionary<int, IList<DataItem>>();
    private CancellationTokenSource _cancellationTokenSource;
    private int _availableDataCount;
    private int _currentPageIndex = 0;


    [Inject]
    public PanelController(IDataServer dataServer, SignalBus signalBus, PanelConfig config)
    {
        _dataServer = dataServer;
        _signalBus = signalBus;
        _config = config;
    }

    public async void Initialize()
    {
        _signalBus.Subscribe<NextPageClickedSignal>(OnNextPageClicked);
        _signalBus.Subscribe<PreviousPageClickedSignal>(OnPreviousPageClicked);

        await LoadPage();
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<NextPageClickedSignal>(OnNextPageClicked);
        _signalBus.Unsubscribe<PreviousPageClickedSignal>(OnPreviousPageClicked);

        CleanupCancellationTokenSource();
    }

    public int GetCurrentPageIndex()
    {
        return _currentPageIndex;
    }

    public ReadOnlyDictionary<int, IList<DataItem>> GetPageCache()
    {
        return new ReadOnlyDictionary<int, IList<DataItem>>(_pageCache);
    }

    public async Task LoadPage()
    {
        CleanupCancellationTokenSource();

        _cancellationTokenSource = new CancellationTokenSource();
        var ct = _cancellationTokenSource.Token;

        try
        {
            _signalBus.Fire(new PageLoadingStateChangedSignal(LoadingState.LoadingInProgress));

            IList<DataItem> pageData;
            if (_pageCache.ContainsKey(_currentPageIndex))
            {
                pageData = _pageCache[_currentPageIndex];
            }
            else
            {
                if (_availableDataCount == 0)
                {
                    _availableDataCount = await _dataServer.DataAvailable(ct);
                }

                int requestCount = GetItemCountToRequest();
                pageData = await _dataServer.RequestData(_currentPageIndex, requestCount, ct);

                if (ct.IsCancellationRequested) return;

                _pageCache[_currentPageIndex] = pageData;
            }

            _signalBus.Fire(new LoadedDataForPageSignal(pageData, _currentPageIndex));
            _signalBus.Fire(new PageLoadingStateChangedSignal(LoadingState.LoadingFinished));
        }
        catch (TaskCanceledException)
        {
            // Ignored - Page load was canceled
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading data: {e.Message}");
        }
    }

    public async Task NavigateNext()
    {
        if (_currentPageIndex + ItemsPerPage < _availableDataCount)
        {
            _currentPageIndex += ItemsPerPage;
            await LoadPage();
            UpdateNavigationState();
        }
    }

    public async Task NavigatePrevious()
    {
        if (_currentPageIndex > 0)
        {
            _currentPageIndex -= ItemsPerPage;
            await LoadPage();
            UpdateNavigationState();
        }
    }

    private async void OnNextPageClicked()
    {
        await NavigateNext();
    }

    private async void OnPreviousPageClicked()
    {
        await NavigatePrevious();
    }

    private void CleanupCancellationTokenSource()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
    }

    private int GetItemCountToRequest()
    {
        return Math.Min(ItemsPerPage, _availableDataCount - _currentPageIndex);
    }

    private void UpdateNavigationState()
    {
        var newState = DetermineNavigationState();
        _signalBus.Fire(new NavigationStateChangedSignal(newState));
    }

    private NavigationState DetermineNavigationState()
    {
        if (_availableDataCount == 0)
            return NavigationState.CannotNavigate;

        if (_currentPageIndex == 0 && _availableDataCount <= ItemsPerPage)
            return NavigationState.CannotNavigate;

        if (_currentPageIndex == 0)
            return NavigationState.CanGoNext;

        if (_currentPageIndex + ItemsPerPage >= _availableDataCount)
            return NavigationState.CanGoPrevious;

        return NavigationState.CanNavigateBothWays;
    }
}