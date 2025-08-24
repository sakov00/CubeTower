using System.Collections.Generic;
using _Project.Scripts._GlobalLogic;
using _Project.Scripts.DraggableObjects;
using _Project.Scripts.Factories;
using _Project.Scripts.Localization;
using _Project.Scripts.Managers;
using _Project.Scripts.ObjectPools;
using _Project.Scripts.Rendering;
using _Project.Scripts.Zones;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using UniRx;

namespace _Project.Scripts.Windows
{
    public class GameWindow : BaseWindow
    {
        [SerializeField] private List<BaseZone> _zones = new();
        [Header("Spawner UI")]
        [SerializeField] private StarSpawner _starSpawner;
        [SerializeField] private LineSpawner _lineSpawner;
        
        [Header("UI elements")]
        [SerializeField] private Button _switchLanguageButton;
        [SerializeField] private TextMeshProUGUI _switchLanguageText;
        [SerializeField] private RectTransform _scrollContent;
        [SerializeField] private Transform _draggableContainer;
        
        [Inject] private DraggablePool _draggablePool;
        [Inject] private DraggableManager _draggableManager;
        [Inject] private LocalizationService _localizationService;

        public override void Initialize()
        {
            _lineSpawner.FillBg();
            _starSpawner.FillBg();
            _draggableManager.OnPointerDowned += ChangeParent;
            _draggableManager.OnEndedDrag += CubeReplaced;
            _draggablePool.SetContainer(_draggableContainer);
            _draggablePool.CreateAllColoredBoxes(_scrollContent);
            
            _localizationService.CurrentLanguage.Subscribe(_ =>
            {
                _switchLanguageText.text = _localizationService.GetText(GameConstants.LocalizationKeys.SwitchLang);
            }).AddTo(this);
            
            _switchLanguageButton.onClick.AddListener(SwitchLanguage);
        }
        
        private void ChangeParent(Draggable draggable)
        {
            draggable.RectTransform.SetParent(transform);
        }

        private void CubeReplaced(Draggable draggable)
        {
            foreach (var zone in _zones)
            {
                var screenPoint = RectTransformUtility.WorldToScreenPoint(null, draggable.RectTransform.position);
                if (RectTransformUtility.RectangleContainsScreenPoint(zone.RectTransform, screenPoint))
                {
                    zone.AddDraggableToZone(draggable);
                }
                else
                {
                    zone.RemoveDraggableFromZone(draggable);
                }
            }
        }

        private void SwitchLanguage()
        {
            if (_localizationService.CurrentLanguage.Value == "En")
            {
                _localizationService.CurrentLanguage.Value = "Ru";
            }
            else if (_localizationService.CurrentLanguage.Value == "Ru")
            {
                _localizationService.CurrentLanguage.Value = "En";
            }
        }

        private void OnDestroy()
        {
            if (_draggableManager != null)
            {
                _draggableManager.OnPointerDowned -= ChangeParent;
                _draggableManager.OnEndedDrag -= CubeReplaced;
            }
            _switchLanguageButton.onClick.RemoveListener(SwitchLanguage);
        }
    }
}