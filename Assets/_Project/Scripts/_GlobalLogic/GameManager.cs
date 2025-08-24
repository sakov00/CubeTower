using _Project.Scripts.Localization;
using _Project.Scripts.Windows;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts._GlobalLogic
{
    public class GameManager : IStartable
    {
        [Inject] private WindowsManager _windowsManager;
        [Inject] private LocalizationService _localizationService;
        
        public void Start()
        {
            Application.targetFrameRate = 120;
            
            // var sequence = DOTween.Sequence();
            // sequence.Append(_windowsManager.ShowWindow<LoadingWindow>());
            // sequence.AppendInterval(2f);
            // sequence.Append(_windowsManager.ShowWindow<GameWindow>());
            // sequence.Append(_windowsManager.HideWindow<LoadingWindow>());
            _windowsManager.ShowWindow<GameWindow>();
        }
    }
}