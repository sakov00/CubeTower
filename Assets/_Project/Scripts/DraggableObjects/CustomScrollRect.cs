using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts._GlobalLogic;

namespace _Project.Scripts.DraggableObjects
{
    public class CustomScrollRect : ScrollRect, IPointerDownHandler
    {
        private bool _lockScroll;
        private bool _directionChosen;
        private GameObject _currentChildObject;

        public void OnPointerDown(PointerEventData eventData)
        {
            _currentChildObject = null;
            _lockScroll = false;
            _directionChosen = false;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (!_directionChosen)
            {
                if (Mathf.Abs(eventData.delta.y) > Mathf.Abs(eventData.delta.x))
                {
                    _lockScroll = true;
                    PassThroughClick(eventData);
                }
                _directionChosen = true;
            }
        
            if (!_lockScroll)
            {
                base.OnBeginDrag(eventData);
            }
        }
        
        public override void OnDrag(PointerEventData eventData)
        {
            if (!_lockScroll)
            {
                base.OnDrag(eventData);
            }
            else
            {
                ExecuteEvents.Execute(_currentChildObject, eventData, ExecuteEvents.dragHandler);
            }
        }
        
        public override void OnEndDrag(PointerEventData eventData)
        {
            _lockScroll = false;
            _directionChosen = false;
            ExecuteEvents.Execute(_currentChildObject, eventData, ExecuteEvents.endDragHandler);
            base.OnEndDrag(eventData);
        }
        
        private void PassThroughClick(PointerEventData eventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            GlobalObjects.EventSystem.RaycastAll(eventData, results);
            _currentChildObject = results[1].gameObject;
            ExecuteEvents.Execute(_currentChildObject, eventData, ExecuteEvents.pointerDownHandler);
            ExecuteEvents.Execute(_currentChildObject, eventData, ExecuteEvents.beginDragHandler);
        }
    }
}
