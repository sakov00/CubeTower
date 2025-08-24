using _Project.Scripts._GlobalLogic;
using _Project.Scripts.DraggableObjects;

namespace _Project.Scripts.Zones
{
    public class TrashZone : BaseZone
    {
        public override void AddDraggableToZone(Draggable draggable)
        {
            _draggablePool.Return(draggable);
            ActionNotifier.PublishAction(GameConstants.LocalizationKeys.BoxThrewOut);
        }
    }
}