using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PanelInstaller", menuName = "Installers/PanelInstaller")]
public class PanelInstaller : ScriptableObjectInstaller<PanelInstaller>
{
    [SerializeField] private PanelConfig _config;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<PanelController>().AsSingle().NonLazy();

        Container.Bind<PanelConfig>().FromInstance(_config).AsSingle(); 

        Container.BindMemoryPool<PanelItem, PanelItemPool>()
            .WithInitialSize(_config.InitialItemsPoolSize)
            .FromComponentInNewPrefab(_config.ItemPrefab)
            .UnderTransformGroup("PanelItemsPool");
    }
}