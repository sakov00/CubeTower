using System;

namespace _Project.Scripts.DraggableObjects
{
    public class DraggableManager : IDisposable
    {
        public Action<Draggable> OnBeginedDrag;
        public Action<Draggable> OnEndedDrag;

        public void Dispose()
        {
            OnBeginedDrag = null;
            OnEndedDrag = null;
        }
    }
}