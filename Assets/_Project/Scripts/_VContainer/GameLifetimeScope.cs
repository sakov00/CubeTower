using _Project.Scripts._GlobalLogic;
using _Project.Scripts.Analytics;
using _Project.Scripts.DraggableObjects;
using _Project.Scripts.Factories;
using _Project.Scripts.FileDatas;
using _Project.Scripts.ObjectPools;
using _Project.Scripts.Registries;
using _Project.Scripts.SO;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts._VContainer
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private ActionNotifier _actionNotifier;
        [SerializeField] private WindowsManager _windowsManager;
        
        [Header("Configs")]
        [SerializeField] private ColoredBoxesConfig _coloredBoxesConfig;
        [SerializeField] private WindowsConfig _windowsConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterBuildCallback(InjectManager.Initialize);
            
            builder.Register<GameManager>(Lifetime.Singleton).AsSelf().As<IAsyncStartable>();
            builder.RegisterInstance(_windowsManager).AsSelf().As<IInitializable>();

            RegisterServices(builder);
            RegisterRegistries(builder);
            RegisterFactories(builder);
            RegisterPools(builder);
            RegisterSO(builder);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.RegisterInstance(_actionNotifier).AsSelf().As<IInitializable>();
            builder.Register<AnalyticsManager>(Lifetime.Singleton).AsSelf();
            builder.Register<LocalizationService>(Lifetime.Singleton).AsSelf();
            builder.Register<DraggableManager>(Lifetime.Singleton).AsSelf();
            builder.Register<LevelManager>(Lifetime.Singleton).AsSelf();
        }
        
        private void RegisterRegistries(IContainerBuilder builder)
        {
            builder.Register<SaveRegistry>(Lifetime.Singleton).AsSelf();
        }
        
        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<DraggableFactory>(Lifetime.Singleton).AsSelf();
        }
        
        private void RegisterPools(IContainerBuilder builder)
        {
            builder.Register<DraggablePool>(Lifetime.Singleton).AsSelf();
        }
        
        private void RegisterSO(IContainerBuilder builder)
        {
            builder.RegisterInstance(_coloredBoxesConfig).AsSelf();
            builder.RegisterInstance(_windowsConfig).AsSelf().As<IInitializable>();
        }
    }
}