using System.Collections.Generic;
using _Project.Scripts.DraggableObjects;
using UnityEngine;

namespace _Project.Scripts.Zones
{
    public class TowerZone : BaseZone
    {
        [SerializeField] private List<Draggable> _draggableObjects = new();

        public override void AddDraggableToZone(Draggable draggable)
        {
            _draggableObjects.Add(draggable);
            draggable.RectTransform.SetParent(transform);
        }
    }
}