using System.Threading;
using _Project.Scripts._VContainer;
using _Project.Scripts.FileDatas;
using _Project.Scripts.ObjectPools;
using _Project.Scripts.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts._GlobalLogic
{
    public class GameManager : IAsyncStartable
    {
        [Inject] private WindowsManager _windowsManager;
        [Inject] private LocalizationService _localizationService;
        [Inject] private LevelManager _levelManager;
        [Inject] private DraggablePool _draggablePool;
        
        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            InjectManager.Inject(this);
            
            Application.targetFrameRate = 120;
            Input.multiTouchEnabled = false;
            
            await _windowsManager.ShowWindow<LoadingWindow>();
            await UniTask.Delay(2000);
            await _localizationService.LoadAsync();
            var gameWindow = _windowsManager.GetWindow<GameWindow>();
            gameWindow.Initialize();
            gameWindow.ShowFast();
            await _levelManager.LoadLevel();
            _windowsManager.GetWindow<GameWindow>().RestoreDataZones();
            _windowsManager.HideWindow<LoadingWindow>();
        }
        
        private void OnApplicationQuit()
        {
            _levelManager?.SaveLevel().Forget();
        }
        
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                _levelManager?.SaveLevel().Forget();
            }
        }
    }
}