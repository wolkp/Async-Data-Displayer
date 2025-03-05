using UnityEngine;

[CreateAssetMenu(fileName = "CategoryIconConfig", menuName = "Configs/CategoryIconConfig")]
public class CategoryIconConfig : ScriptableObject
{
    [System.Serializable]
    public struct CategoryIconMapping
    {
        public DataItem.CategoryType category;
        public Sprite categoryIcon;
    }

    [SerializeField] private CategoryIconMapping[] categoryIconMappings;

    public Sprite GetIconForCategory(DataItem.CategoryType category)
    {
        foreach (var mapping in categoryIconMappings)
        {
            if (mapping.category == category)
            {
                return mapping.categoryIcon;
            }
        }

        return null;
    }
}