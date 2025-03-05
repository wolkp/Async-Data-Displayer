using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PanelSignalsInstaller", menuName = "Installers/PanelSignalsInstaller")]
public class PanelSignalsInstaller : ScriptableObjectInstaller<PanelSignalsInstaller>
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<LoadedDataForPageSignal>();
        Container.DeclareSignal<PageLoadingStateChangedSignal>();
        Container.DeclareSignal<NavigationStateChangedSignal>();

        Container.DeclareSignal<NextPageClickedSignal>();
        Container.DeclareSignal<PreviousPageClickedSignal>();
    }
}