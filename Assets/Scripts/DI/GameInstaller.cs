using Presenter;
using Zenject;

public class GameInstaller : MonoInstaller
{
  public override void InstallBindings()
  {
    ContainerHolder.container = Container;

    Container.BindInterfacesAndSelfTo<PoolableComponentInjectFactory>().AsSingle();
    Container.Bind<PoolManager>().FromNew().AsSingle();

    Container.Bind<IGroundPresenter>().FromComponentInHierarchy().AsSingle();
    Container.Bind<IHousePresenter>().FromComponentInHierarchy().AsSingle();
  }
}
