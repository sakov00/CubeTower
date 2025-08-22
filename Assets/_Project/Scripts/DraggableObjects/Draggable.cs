using _Project.Scripts._GlobalLogic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Scripts.DraggableObjects
{
    [RequireComponent(typeof(RectTransform))]
    public class Draggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform _rectTransform;

        private Vector3 _offset;
        private bool _isDragging;

        private void OnValidate()
        {
            _rectTransform ??= GetComponent<RectTransform>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
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
                    _rectTransform, eventData.position, GlobalObjects.Camera, out var worldPoint))
            {
                _rectTransform.position = worldPoint + _offset;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
        }
    }
}
