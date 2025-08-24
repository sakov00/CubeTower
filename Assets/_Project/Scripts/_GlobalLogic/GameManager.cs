using _Project.Scripts._VContainer;
using _Project.Scripts.Managers;
using _Project.Scripts.Windows;
using UnityEngine;
using VContainer;

namespace _Project.Scripts._GlobalLogic
{
    public class GameManager : MonoBehaviour
    {
        [Inject] private WindowsManager _windowsManager;
        [Inject] private LevelManager _levelManager;
        
        public void Start()
        {
            InjectManager.Inject(this);
            Application.targetFrameRate = 120;
            
            // var sequence = DOTween.Sequence();
            // sequence.Append(_windowsManager.ShowWindow<LoadingWindow>());
            // sequence.AppendInterval(2f);
            // sequence.Append(_windowsManager.ShowWindow<GameWindow>());
            // sequence.Append(_windowsManager.HideWindow<LoadingWindow>());
            _windowsManager.ShowWindow<GameWindow>();
            _levelManager.LoadLevel();
        }
        
        private void OnApplicationQuit()
        {
            _levelManager.SaveLevel();
        }
        
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                _levelManager.SaveLevel();
            }
        }
    }
}