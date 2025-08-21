using _Project.Scripts.Windows;
using DG.Tweening;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts._GlobalLogic
{
    public class GameManager : IStartable
    {
        [Inject] private WindowsManager _windowsManager;
        
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