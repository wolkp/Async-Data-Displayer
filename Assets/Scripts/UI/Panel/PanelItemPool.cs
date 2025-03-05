using UnityEngine;
using Zenject;

public class PanelItemPool : MonoMemoryPool<int, DataItem, PanelItem>
{
    protected override void Reinitialize(int index, DataItem dataItem, PanelItem panelItem)
    {
        panelItem.Initialize(index, dataItem);
    }
}