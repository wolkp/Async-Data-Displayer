using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class PanelView : MonoBehaviour
{
    [Inject] private readonly PanelItemPool _panelItemPool;
    [Inject] private readonly SignalBus _signalBus;

    [SerializeField] private Transform _dataItemsContainer;

    private readonly List<PanelItem> _activePanelItems = new();

    private void OnEnable()
    {
        _signalBus.Subscribe<LoadedDataForPageSignal>(OnLoadedDataForPage);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<LoadedDataForPageSignal>(OnLoadedDataForPage);
    }

    private void OnLoadedDataForPage(LoadedDataForPageSignal signal)
    {
        UpdateDataItems(signal.DataItems, signal.PageIndex);
    }

    private void UpdateDataItems(IList<DataItem> dataItems, int pageIndex)
    {
        ClearExistingPanels();
        SpawnPanelsForData(dataItems, pageIndex);
    }

    private void ClearExistingPanels()
    {
        foreach (var panelItem in _activePanelItems)
        {
            _panelItemPool.Despawn(panelItem);
        }
        _activePanelItems.Clear();
    }

    private void SpawnPanelsForData(IList<DataItem> dataItems, int pageIndex)
    {
        for(int i = 0; i < dataItems.Count; i++)
        {
            int itemIndex = pageIndex + i;
            DataItem dataItem = dataItems[i];
            PanelItem panelItem = _panelItemPool.Spawn(itemIndex, dataItem);
            panelItem.transform.SetParent(_dataItemsContainer, false);
            _activePanelItems.Add(panelItem);
        }
    }
}