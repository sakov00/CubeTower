using System.Collections.Generic;
using _Project.Scripts._VContainer;
using _Project.Scripts.DraggableObjects;
using _Project.Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Zones
{
    public class TowerZone : BaseZone
    {
        [SerializeField] private List<Draggable> _draggableObjects = new();
        [SerializeField] private float _percentOffsetBox = 0.5f;
        
        [Inject] private DraggableManager _draggableManager;

        private void Awake()
        {
            InjectManager.Inject(this);
            _draggableManager.OnPointerDowned += RemoveDraggableFromZone;
        }

        public override void AddDraggableToZone(Draggable draggable)
        {
            UpdateRectTransform(draggable.RectTransform);
            SetPosition(draggable);
        }

        private void UpdateRectTransform(RectTransform rectTransform)
        {
            rectTransform.SetParent(transform);
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.zero;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        private void SetPosition(Draggable draggable)
        {
            var zoneLeft = draggable.RectTransform.rect.width / 2;
            var zoneRight = RectTransform.rect.width - draggable.RectTransform.rect.width / 2;
            
            if (_draggableObjects.Count == 0)
            {
                var newX = Mathf.Clamp(draggable.RectTransform.anchoredPosition.x, zoneLeft, zoneRight);
                draggable.RectTransform.anchoredPosition = new Vector2(newX, draggable.RectTransform.rect.height * 0.5f);
            }
            else
            {
                var last = _draggableObjects[^1];
                var newY = last.RectTransform.anchoredPosition.y + last.RectTransform.rect.height;
                
                var zoneHeight = RectTransform.rect.height;
                if (newY > zoneHeight && draggable is ColoredBox coloredBox)
                {
                    _coloredBoxesPool.Return(coloredBox);
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

            _draggableObjects.Add(draggable);
        }

        public override void RemoveDraggableFromZone(Draggable draggable)
        {
            var index = _draggableObjects.IndexOf(draggable);
            if (index == -1) return;
            
            _draggableObjects.RemoveAt(index);
            
            for (var i = index; i < _draggableObjects.Count; i++)
            {
                var newY = _draggableObjects[i].RectTransform.anchoredPosition.y - draggable.RectTransform.rect.height;
                _draggableObjects[i].RectTransform.DOAnchorPosY(newY, 0.2f);
            }
        }

        private void OnDestroy()
        {
            _draggableManager.OnPointerDowned -= RemoveDraggableFromZone;
        }
    }
}