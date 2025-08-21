using System;
using _Project.Scripts.Enums;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Scripts.Windows
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BaseWindow : MonoBehaviour, IInitializable
    {
        [SerializeField] private WindowLayerType _windowLayerType;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public WindowLayerType WindowLayerType => _windowLayerType;

        private void OnValidate()
        {
            _canvasGroup ??= GetComponent<CanvasGroup>();
        }
        
        public virtual void Initialize()
        {
        }

        public virtual Tween Show()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(1f, 0.5f).From(0));
            return sequence;
        }

        public virtual Tween Hide()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(0f, 0.5f).From(1));
            return sequence;
        }
    }
}