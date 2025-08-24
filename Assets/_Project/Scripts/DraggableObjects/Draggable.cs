using System;
using _Project.Scripts._GlobalLogic;
using _Project.Scripts.Helpers;
using _Project.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace _Project.Scripts.DraggableObjects
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class Draggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [field: SerializeField] public RectTransform RectTransform { get; private set; }
        
        [Inject] private DraggableManager _draggableManager;

        private Vector3 _offset;
        private bool _isDragging;
        
        private event Action<Draggable> OnPointerDowned;
        private event Action<Draggable> OnEndedDrag;

        private void OnValidate()
        {
            RectTransform ??= GetComponent<RectTransform>();
        }

        public void Initialize()
        {
            OnPointerDowned += _draggableManager.OnPointerDowned;
            OnEndedDrag += _draggableManager.OnEndedDrag;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDowned?.Invoke(this);
            _isDragging = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _offset = Vector3.zero;
            _isDragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    RectTransform, eventData.position, GlobalObjects.Camera, out var worldPoint))
            {
                RectTransform.position = worldPoint + _offset;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            OnEndedDrag?.Invoke(this);
        }

        private void OnDestroy()
        {
            OnPointerDowned = null;
            OnEndedDrag = null;
            OnPointerDowned -= _draggableManager?.OnPointerDowned;
            OnEndedDrag -= _draggableManager?.OnEndedDrag;
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
    }
}
