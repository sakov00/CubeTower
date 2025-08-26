using System.Collections.Generic;
using System.Linq;
using _Project.Scripts._GlobalLogic;
using _Project.Scripts._VContainer;
using _Project.Scripts.DraggableObjects;
using _Project.Scripts.Extensions;
using _Project.Scripts.Registries;
using Cysharp.Threading.Tasks;
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
        
        [Inject] private SaveRegistry _saveRegistry;
        [Inject] private DraggableManager _draggableManager;

        public override void Initialize()
        {
            base.Initialize();
            _draggableManager.OnBeginedDrag += RemoveDraggableFromZone;
        }

        public override void RestoreData()
        {
            _towerObjects.Clear();

            if (transform.childCount <= 0) 
                return;
            
            foreach (var coloredBox in _saveRegistry.GetAll<ColoredBox>())
            {
                if (coloredBox.RectTransform.parent == transform)
                {
                    _towerObjects.Add(coloredBox);
                }
            }
            _towerObjects.Sort((a, b) =>
                a.RectTransform.anchoredPosition.y.CompareTo(b.RectTransform.anchoredPosition.y));
        }

        public override async UniTask AddDraggableToZone<T>(T draggable)
        {
            if (CheckHitToNextPos(draggable) || _towerObjects.Count == 0)
            {
                UpdateRectTransform(draggable.RectTransform);
                SetPosition(draggable).Forget();
            }
            else
            {
                await draggable.Hide();
                DraggablePool.Return(draggable);
                ActionNotifier.PublishAction(GameConstants.LocalizationKeys.BoxMissed);
            }
        }

        private bool CheckHitToNextPos(Draggable draggable)
        {
            var last = _towerObjects.LastOrDefault();
            if(last == null)
                return false;

            return draggable.RectTransform.IsOverlappingAnchored(last.RectTransform, 0,
                draggable.RectTransform.rect.height, _extraRadius);
        }

        private void UpdateRectTransform(RectTransform rectTransform)
        {
            Vector3 worldPos = rectTransform.position;
            rectTransform.DOKill();
            rectTransform.SetParent(transform, worldPositionStays: true);
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.zero;
            rectTransform.position = worldPos;
        }

        private async UniTask SetPosition(Draggable draggable)
        {
            var zoneLeft = draggable.RectTransform.rect.width / 2;
            var zoneRight = RectTransform.rect.width - draggable.RectTransform.rect.width / 2;

            if (_towerObjects.Count == 0)
            {
                var newX = Mathf.Clamp(draggable.RectTransform.anchoredPosition.x, zoneLeft, zoneRight);
                var newY = draggable.RectTransform.rect.height * 0.5f;
                draggable.RealPos = new Vector2(newX, newY);
            }
            else
            {
                var last = _towerObjects[^1];
                var newY = last.RealPos.y + last.RectTransform.rect.height;

                var zoneHeight = RectTransform.rect.height;
                if (newY > zoneHeight)
                {
                    await draggable.Hide();
                    DraggablePool.Return((ColoredBox)draggable);
                    ActionNotifier.PublishAction(GameConstants.LocalizationKeys.MaximumAltitudeExceeded);
                    return;
                }

                var halfWidth = draggable.RectTransform.rect.width * _percentOffsetBox;
                var randomXOffset = Random.Range(-halfWidth, halfWidth);
                var newX = Mathf.Clamp(last.RectTransform.anchoredPosition.x + randomXOffset, zoneLeft, zoneRight);

                draggable.RealPos = new Vector2(newX, newY);
            }
            
            draggable.RectTransform.DOKill();
            draggable.RectTransform.DOJumpAnchorPos(draggable.RealPos, 100f, 1, 0.5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => draggable.RectTransform.anchoredPosition = draggable.RealPos);

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
                _towerObjects[i].RectTransform.DOKill();
                var newY = _towerObjects[i].RealPos.y - draggable.RectTransform.rect.height;
                _towerObjects[i].RealPos = new Vector2(_towerObjects[i].RealPos.x, newY);
                _towerObjects[i].RectTransform.DOAnchorPosY(newY, 0.2f)
                    .SetEase(Ease.OutQuad);
            }
        }

        private void OnDestroy()
        {
            if (_draggableManager != null)
            {
                _draggableManager.OnBeginedDrag -= RemoveDraggableFromZone;
            }
        }
    }
}