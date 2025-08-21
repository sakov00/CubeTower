using _Project.Scripts._GlobalLogic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.DraggableObjects
{
    [RequireComponent(typeof(RectTransform))]
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        private Vector3 _offset;

        private void OnValidate()
        {
            _rectTransform ??= GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTransform,
                    eventData.position, GlobalObjects.Camera, out var worldPoint))
            {
                _offset = _rectTransform.position - worldPoint;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTransform,
                    eventData.position, GlobalObjects.Camera, out var worldPoint))
            {
                _rectTransform.position = worldPoint + _offset;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // Можно добавить логику по завершению перетаскивания
        }
    }
}