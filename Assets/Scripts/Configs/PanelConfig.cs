using UnityEngine;

[CreateAssetMenu(fileName = "PanelConfig", menuName = "Configs/PanelConfig")]
public class PanelConfig : ScriptableObject
{
    [SerializeField] private PanelItem _itemPrefab;
    [SerializeField] private int _itemsPerPage = 5;
    [SerializeField] private int _initialItemsPoolSize = 25;

    public PanelItem ItemPrefab => _itemPrefab;
    public int ItemsPerPage => _itemsPerPage;
    public int InitialItemsPoolSize => _initialItemsPoolSize;
}