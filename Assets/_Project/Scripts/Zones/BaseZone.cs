using System;
using System.Collections.Generic;
using _Project.Scripts._VContainer;
using _Project.Scripts.Analytics;
using _Project.Scripts.DraggableObjects;
using _Project.Scripts.Managers;
using _Project.Scripts.ObjectPools;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.Zones
{
    public abstract class BaseZone : MonoBehaviour
    {
        [SerializeField] protected DraggablePool _draggablePool;
        [field: SerializeField] public RectTransform RectTransform { get; private set; }
        
        [Inject] protected ActionNotifier ActionNotifier;

        private void OnValidate()
        {
            RectTransform ??= GetComponent<RectTransform>();
        }

        protected virtual void Awake()
        {
            InjectManager.Inject(this);
        }

        public abstract void AddDraggableToZone(Draggable draggable);
        public virtual void RemoveDraggableFromZone(Draggable draggable) { }
    }
}