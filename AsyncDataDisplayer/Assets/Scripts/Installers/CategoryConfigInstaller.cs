using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CategoryConfigInstaller", menuName = "Installers/CategoryConfigInstaller")]
public class CategoryConfigInstaller : ScriptableObjectInstaller<CategoryConfigInstaller>
{
    [SerializeField] private CategoryIconConfig _config;

    public override void InstallBindings()
    {
        Container.Bind<CategoryIconConfig>().FromInstance(_config).AsSingle();
    }
}