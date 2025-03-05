using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _indexText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Image _specialIndicator;
    [SerializeField] private CategoryIconSetter _categoryIconSetter;

    public void Initialize(int index, DataItem dataItem)
    {
        _indexText.text = $"{index + 1}";
        _descriptionText.text = dataItem.Description;

        _specialIndicator.gameObject.SetActive(dataItem.Special);

        _categoryIconSetter.SetCategoryIcon(dataItem.Category);
    }
}