using _Project.Scripts.DraggableObjects;
using _Project.Scripts.ObjectPools;
using UnityEngine;

namespace _Project.Scripts.Zones
{
    public class TrashZone : BaseZone
    {
        [SerializeField] private ColoredBoxesPool _coloredBoxesPool;
        public override void AddDraggableToZone(Draggable draggable)
        {
            if(draggable is ColoredBox coloredBox)
                _coloredBoxesPool.Return(coloredBox);
        }
    }
}