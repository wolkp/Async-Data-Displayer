using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "DataInstaller", menuName = "Installers/DataInstaller")]
public class DataInstaller : ScriptableObjectInstaller<DataInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IDataServer>().To<DataServerMock>().AsSingle();
    }
}