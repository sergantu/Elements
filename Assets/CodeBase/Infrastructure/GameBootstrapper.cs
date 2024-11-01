using CodeBase.GameLevel;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using CodeBase.PersistentProgress;
using CodeBase.SaveLoad.Service;
using CodeBase.StaticData.Service;
using CodeBase.UI.Service;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<InitialEntryPoint>(Lifetime.Singleton);

            builder.Register<IStaticDataService, StaticDataService>(Lifetime.Singleton);
            builder.Register<IPersistentProgressService, PersistentProgressService>(Lifetime.Singleton);
            builder.Register<SaveLoadService>(Lifetime.Singleton);
            builder.Register<UIService>(Lifetime.Singleton);
            builder.Register<SceneLoader>(Lifetime.Singleton);
            builder.Register<AssetProvider>(Lifetime.Singleton);
            builder.Register<GameFactory>(Lifetime.Singleton);

            builder.Register<GameStateMachine>(Lifetime.Singleton);
            builder.Register<BootstrapState>(Lifetime.Transient);
            builder.Register<LoadProgressState>(Lifetime.Transient);
            builder.Register<LoadLevelState>(Lifetime.Transient);
            builder.Register<GameLoopState>(Lifetime.Transient);
            
            
            builder.Register<ElementsGenerator>(Lifetime.Singleton);
            builder.Register<GameLogicService>(Lifetime.Singleton);
            builder.Register<MoveBlockService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        }

        /*private void HandleException(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Exception) {
                Debug.LogError("Unhandled exception: " + condition + ", stackTrace: \n" + stackTrace);
            }
        }

        private void HandleUniTaskException(Exception ex) => Debug.LogError("Unhandled UniTask exception: " + ex);*/
    }
}