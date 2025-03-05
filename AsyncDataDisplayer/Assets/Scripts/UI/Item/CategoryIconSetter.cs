using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Image))]
public class CategoryIconSetter : MonoBehaviour
{
    [Inject] private CategoryIconConfig _categoryIconConfig;

    private Image _categoryIconImage;

    private void Awake()
    {
        _categoryIconImage = GetComponent<Image>();
    }

    public void SetCategoryIcon(DataItem.CategoryType category)
    {
        Sprite icon = _categoryIconConfig.GetIconForCategory(category);

        if (icon != null)
        {
            _categoryIconImage.sprite = icon;
            _categoryIconImage.gameObject.SetActive(true);
        }
        else
        {
            _categoryIconImage.gameObject.SetActive(false);
        }
    }
}