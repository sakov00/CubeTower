using _Project.Scripts._GlobalLogic;
using _Project.Scripts.Analytics;
using _Project.Scripts.Factories;
using _Project.Scripts.Localization;
using _Project.Scripts.Managers;
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
            
            builder.Register<GameManager>(Lifetime.Singleton).As<GameManager, IStartable>();
            
            builder.RegisterInstance(_windowsManager).AsSelf().As<IInitializable>();

            RegisterServices(builder);
            RegisterFactories(builder);
            RegisterSO(builder);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.RegisterInstance(_actionNotifier).AsSelf().As<IInitializable>();
            builder.Register<AnalyticsManager>(Lifetime.Singleton).AsSelf();
            builder.Register<LocalizationService>(Lifetime.Singleton).AsSelf();
            builder.Register<DraggableManager>(Lifetime.Singleton).AsSelf();
        }
        
        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<DraggableFactory>(Lifetime.Singleton).AsSelf();
        }
        
        private void RegisterSO(IContainerBuilder builder)
        {
            builder.RegisterInstance(_coloredBoxesConfig).AsSelf();
            builder.RegisterInstance(_windowsConfig).AsSelf().As<IInitializable>();
        }
    }
}