using _Project.Scripts.DraggableObjects;

namespace _Project.Scripts.Zones
{
    public class TrashZone : BaseZone
    {
        public override void AddDraggableToZone(Draggable draggable)
        {
            if(draggable is ColoredBox coloredBox)
                _coloredBoxesPool.Return(coloredBox);
        }
    }
}