using System;
using System.Collections.Generic;
using _Project.Scripts._VContainer;
using _Project.Scripts.Enums;
using _Project.Scripts.SO;
using _Project.Scripts.Windows;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts._GlobalLogic
{
    public class WindowsManager : MonoBehaviour, IInitializable
    {
        [Header("Layers")]
        [SerializeField] private RectTransform _windowsLayer;
        [SerializeField] private RectTransform _popupLayer;
        [SerializeField] private RectTransform _otherLayer;
        
        [Header("Layers")]
        [SerializeField] private Image _darkBackground;
        
        [Inject] private IObjectResolver _resolver;
        [Inject] private WindowsConfig _windowsConfig;
        
        private readonly Dictionary<Type, BaseWindow> _cachedWindows = new ();
        
        public void Initialize()
        {
            InjectManager.Inject(this);
        }
        
        public T GetWindow<T>() where T : BaseWindow
        {
            var windowType = _windowsConfig.Windows[typeof(T)].WindowLayerType;
            if (!_cachedWindows.TryGetValue(typeof(T), out var window))
            {
                window = _resolver.Instantiate(_windowsConfig.Windows[typeof(T)], parent: GetParent(windowType));
                _cachedWindows.Add(typeof(T), window);
            }

            return (T)window;
        }
        
        public Tween ShowWindow<T>() where T : BaseWindow
        {
            var windowType = _windowsConfig.Windows[typeof(T)].WindowLayerType;
            if (!_cachedWindows.TryGetValue(typeof(T), out var window))
            {
                window = _resolver.Instantiate(_windowsConfig.Windows[typeof(T)], parent: GetParent(windowType));
                window.Initialize();
                _cachedWindows.Add(typeof(T), window);
            }
            
            var sequence = DOTween.Sequence();
            sequence.Append(window.Show());
            if (windowType == WindowLayerType.Popup) sequence.Join(ShowDarkBackground());
            sequence.SetUpdate(true);
            return sequence;
        }
        
        public Tween HideWindow<T>() where T : BaseWindow
        {
            var windowType = _windowsConfig.Windows[typeof(T)].WindowLayerType;
            if (!_cachedWindows.TryGetValue(typeof(T), out var window))
            {
                window = _resolver.Instantiate(_windowsConfig.Windows[typeof(T)], parent: GetParent(windowType));
                window.Initialize();
                _cachedWindows.Add(typeof(T), window);
            }
    
            var sequence = DOTween.Sequence();
            sequence.Append(window.Hide());
            if (windowType == WindowLayerType.Popup) sequence.Join(HideDarkBackground());
            sequence.SetUpdate(true);
            return sequence;
        }
        
        private Tween ShowDarkBackground()
        {
            var color = _darkBackground.color;
            color.a = 0.5f;
            var sequence = DOTween.Sequence();
            sequence.AppendCallback(() => _darkBackground.gameObject.SetActive(true));
            sequence.Append(_darkBackground.DOColor(color, 0.5f));
            return sequence;
        }
        
        private Tween HideDarkBackground()
        {
            var color = _darkBackground.color;
            color.a = 0f;
            var sequence = DOTween.Sequence();
            sequence.Append(_darkBackground.DOColor(color, 0.5f));
            sequence.AppendCallback(() => _darkBackground.gameObject.SetActive(false));
            return sequence;
        }
        
        private Transform GetParent(WindowLayerType type)
        {
            return type switch
            {
                WindowLayerType.Window => _windowsLayer,
                WindowLayerType.Popup => _popupLayer,
                WindowLayerType.Other => _otherLayer,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        
    }
}