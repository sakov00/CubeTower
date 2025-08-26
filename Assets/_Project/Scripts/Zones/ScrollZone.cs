using _Project.Scripts._GlobalLogic;
using _Project.Scripts.DraggableObjects;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Zones
{
    public class ScrollZone : BaseZone
    {
        public override async UniTask AddDraggableToZone<T>(T draggable)
        {
            await draggable.Hide();
            DraggablePool.Return(draggable);
            ActionNotifier.PublishAction(GameConstants.LocalizationKeys.BoxMissed);
        }
    }
}