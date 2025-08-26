using _Project.Scripts._GlobalLogic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Zones
{
    public class TrashZone : BaseZone
    {
        [SerializeField] private RectTransform _trashArea;
        [SerializeField] private RectTransform _trashAreaMasked;

        public override async UniTask AddDraggableToZone<T>(T draggable)
        {
            UpdateRectTransform(draggable.RectTransform);

            var targetPosition = _trashArea.anchoredPosition + Vector2.up * _trashArea.rect.height * 0.5f;
            
            var sequence = DOTween.Sequence();
            sequence.Append(draggable.RectTransform
                .DOJumpAnchorPos(targetPosition, 200f, 1, 0.5f)
                .SetEase(Ease.OutQuad)); 
            sequence.AppendCallback(() =>
            {
                var worldPos = draggable.RectTransform.position;
                draggable.RectTransform.SetParent(_trashAreaMasked, worldPositionStays: true);
                draggable.RectTransform.position = worldPos;
            });
            sequence.Append(draggable.RectTransform
                .DOAnchorPosY(-200f, 0.2f)
                .SetEase(Ease.Linear));

            await sequence.Play();

            DraggablePool.Return(draggable);
            ActionNotifier.PublishAction(GameConstants.LocalizationKeys.BoxThrewOut);
        }
        
        private void UpdateRectTransform(RectTransform rectTransform)
        {
            var worldPos = rectTransform.position;
            rectTransform.DOKill();
            rectTransform.SetParent(transform, worldPositionStays: true);
            rectTransform.anchorMin = new Vector2(0.5f, 0f);
            rectTransform.anchorMax = new Vector2(0.5f, 0f);
            rectTransform.position = worldPos;
        }
    }
}