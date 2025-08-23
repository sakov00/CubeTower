using System;
using System.Collections.Generic;
using _Project.Scripts.DraggableObjects;
using _Project.Scripts.ObjectPools;
using UnityEngine;

namespace _Project.Scripts.Zones
{
    public abstract class BaseZone : MonoBehaviour
    {
        [SerializeField] protected ColoredBoxesPool _coloredBoxesPool;
        [field: SerializeField] public RectTransform RectTransform { get; private set; }

        private void OnValidate()
        {
            RectTransform ??= GetComponent<RectTransform>();
        }

        public abstract void AddDraggableToZone(Draggable draggable);
        public virtual void RemoveDraggableFromZone(Draggable draggable) { }
    }
}