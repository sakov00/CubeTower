using System.Collections.Generic;
using _Project.Scripts._GlobalLogic;
using _Project.Scripts._VContainer;
using _Project.Scripts.DraggableObjects;
using _Project.Scripts.Helpers;
using _Project.Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Zones
{
    public class TowerZone : BaseZone
    {
        [SerializeField] private List<Draggable> _towerObjects = new();
        [SerializeField] private float _percentOffsetBox = 0.5f;
        [SerializeField] private float _extraRadius = 50f;
        
        [Inject] private DraggableManager _draggableManager;

        protected override void Awake()
        {
            base.Awake();
            _draggableManager.OnPointerDowned += RemoveDraggableFromZone;
        }

        public override void AddDraggableToZone(Draggable draggable)
        {
            if (CheckHitToTower(draggable) || _towerObjects.Count == 0)
            {
                UpdateRectTransform(draggable.RectTransform);
                SetPosition(draggable);
            }
            else
            {
                _draggablePool.Return(draggable);
                ActionNotifier.PublishAction(GameConstants.LocalizationKeys.BoxMissed);
            }
        }

        private bool CheckHitToTower(Draggable draggable)
        {
            foreach (var towerObject in _towerObjects)
            {
                if (towerObject.RectTransform.IsOverlappingAnchored(draggable.RectTransform, _extraRadius))
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateRectTransform(RectTransform rectTransform)
        {
            rectTransform.SetParent(transform);
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.zero;
        }

        private void SetPosition(Draggable draggable)
        {
            var zoneLeft = draggable.RectTransform.rect.width / 2;
            var zoneRight = RectTransform.rect.width - draggable.RectTransform.rect.width / 2;
            
            if (_towerObjects.Count == 0)
            {
                var newX = Mathf.Clamp(draggable.RectTransform.anchoredPosition.x, zoneLeft, zoneRight);
                draggable.RectTransform.anchoredPosition = new Vector2(newX, draggable.RectTransform.rect.height * 0.5f);
            }
            else
            {
                var last = _towerObjects[^1];
                var newY = last.RectTransform.anchoredPosition.y + last.RectTransform.rect.height;
                
                var zoneHeight = RectTransform.rect.height;
                if (newY > zoneHeight && draggable is ColoredBox coloredBox)
                {
                    _draggablePool.Return(coloredBox);
                    ActionNotifier.PublishAction(GameConstants.LocalizationKeys.MaximumAltitudeExceeded);
                    return;
                }

                var halfWidth = draggable.RectTransform.rect.width * _percentOffsetBox;
                var randomXOffset = Random.Range(-halfWidth, halfWidth);
                var newX = Mathf.Clamp(last.RectTransform.anchoredPosition.x + randomXOffset, 
                    zoneLeft, zoneRight);
                
                draggable.RectTransform.anchoredPosition = new Vector2(newX, newY);

                draggable.RectTransform.DOAnchorPosY(newY + 30f, 0.2f)
                    .OnComplete(() =>
                        draggable.RectTransform.DOAnchorPosY(newY, 0.2f)
                    );
            }
            
            _towerObjects.Add(draggable);
            ActionNotifier.PublishAction(GameConstants.LocalizationKeys.BoxInstalled);
        }

        public override void RemoveDraggableFromZone(Draggable draggable)
        {
            var index = _towerObjects.IndexOf(draggable);
            if (index == -1) return;
            
            _towerObjects.RemoveAt(index);
            
            for (var i = index; i < _towerObjects.Count; i++)
            {
                var newY = _towerObjects[i].RectTransform.anchoredPosition.y - draggable.RectTransform.rect.height;
                _towerObjects[i].RectTransform.DOAnchorPosY(newY, 0.2f);
            }
        }

        private void OnDestroy()
        {
            if (_draggableManager != null)
            {
                _draggableManager.OnPointerDowned -= RemoveDraggableFromZone;
            }
        }
    }
}