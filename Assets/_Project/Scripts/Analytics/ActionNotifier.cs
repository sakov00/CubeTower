using System;
using System.Threading;
using _Project.Scripts._VContainer;
using _Project.Scripts.FileDatas;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Analytics
{
    public class ActionNotifier : MonoBehaviour, IInitializable
    {
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private float _displayTime = 2f;
        
        [Inject] private AnalyticsManager _analyticsManager;
        [Inject] private LocalizationService _localizationService;
        
        private string _currentKey;
        private CancellationTokenSource _cts;
        
        public event Action<string> OnAction;

        public void Initialize()
        {
            InjectManager.Inject(this);

            _localizationService.CurrentLanguage.Subscribe(_ =>
            {
                if(!string.IsNullOrEmpty(_currentKey))
                    _messageText.text = _localizationService.GetText(_currentKey);
            }).AddTo(this);
            
            OnAction += async action =>
            {
                await ShowMessageAsync(action);
            };
        }

        private async UniTask ShowMessageAsync(string message)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            _messageText.text = message;
            _messageText.gameObject.SetActive(true);

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_displayTime), cancellationToken: token);
            }
            catch (OperationCanceledException) { }
            
            if (!token.IsCancellationRequested)
                _messageText.gameObject.SetActive(false);
        }

        public void PublishAction(string action)
        {
            _currentKey = action;
            var localizedText = _localizationService.GetText(action);
            OnAction?.Invoke(localizedText);
            _analyticsManager.LogAction(action); 
        }

        private void OnDestroy()
        {
            OnAction = null;
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}