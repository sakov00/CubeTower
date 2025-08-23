using System;
using System.Collections.Generic;
using _Project.Scripts.DraggableObjects;
using UnityEngine;

namespace _Project.Scripts.Zones
{
    public class BaseZone : MonoBehaviour
    {
        [field: SerializeField] public RectTransform RectTransform { get; private set; }

        private void OnValidate()
        {
            RectTransform ??= GetComponent<RectTransform>();
        }

        public virtual void AddDraggableToZone(Draggable draggable)
        {
            
        }
    }
}