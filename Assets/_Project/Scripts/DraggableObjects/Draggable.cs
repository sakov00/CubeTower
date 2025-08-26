using System;
using _Project.Scripts._GlobalLogic;
using _Project.Scripts.Extensions;
using _Project.Scripts.FileDatas;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace _Project.Scripts.DraggableObjects
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Draggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [field: SerializeField] public RectTransform RectTransform { get; private set; }
        [field: SerializeField] public CanvasGroup CanvasGroup { get; set; }
        [field: NonSerialized] public Vector3 RealPos { get; set; }
        
        [Inject] private DraggableManager _draggableManager;

        private readonly Vector3 _offset = new (0, 200, 0);
        private bool _isDragging;
        
        private event Action<Draggable> OnBeginedDrag;
        private event Action<Draggable> OnEndedDrag;

        private void OnValidate()
        {
            RectTransform ??= GetComponent<RectTransform>();
            CanvasGroup ??= GetComponent<CanvasGroup>();
        }

        public void Initialize()
        {
            OnBeginedDrag += _draggableManager.OnBeginedDrag;
            OnEndedDrag += _draggableManager.OnEndedDrag;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            RectTransform.DOKill();
            _isDragging = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            RectTransform.DOKill();
            _isDragging = true;

            UpdatePosition(eventData);
            OnBeginedDrag?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            UpdatePosition(eventData);
        }
        
        private void UpdatePosition(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    RectTransform, eventData.position, GlobalObjects.Camera, out var worldPoint))
            {
                Vector3 worldOffset = RectTransform.TransformVector(_offset);
                var pos = worldPoint + worldOffset;

                var halfWidth = RectTransform.rect.width * 0.5f * RectTransform.lossyScale.x;
                var halfHeight = RectTransform.rect.height * 0.5f * RectTransform.lossyScale.y;

                var min = GlobalObjects.Camera.ScreenToWorldPoint(new Vector3(0, 0, GlobalObjects.Camera.nearClipPlane));
                var max = GlobalObjects.Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, GlobalObjects.Camera.nearClipPlane));

                pos.x = Mathf.Clamp(pos.x, min.x + halfWidth, max.x - halfWidth);
                pos.y = Mathf.Clamp(pos.y, min.y + halfHeight, max.y - halfHeight);

                RectTransform.position = pos;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            OnEndedDrag?.Invoke(this);
        }

        public Tween Hide()
        {
            return CanvasGroup.DOFade(0f, 0.5f);
        }
        
        public virtual DraggableData GetJsonData()
        {
            return new DraggableData
            {
                AnchoredPosition = RectTransform.anchoredPosition,
                AnchorMax = RectTransform.anchorMax,
                AnchorMin = RectTransform.anchorMin,
                ParentPath = RectTransform.parent?.GetFullPath()
            };
        }

        public virtual void SetJsonData(DraggableData data)
        {
            RectTransform.anchoredPosition = data.AnchoredPosition;
            RectTransform.anchorMax = data.AnchorMax;
            RectTransform.anchorMin = data.AnchorMin;
        }

        private void OnDestroy()
        {
            OnBeginedDrag = null;
            OnEndedDrag = null;
            OnBeginedDrag -= _draggableManager?.OnBeginedDrag;
            OnEndedDrag -= _draggableManager?.OnEndedDrag;
        }
    }
}
