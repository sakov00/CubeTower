using System;
using _Project.Scripts.DraggableObjects;

namespace _Project.Scripts.Managers
{
    public class DraggableManager : IDisposable
    {
        public Action<Draggable> OnPointerDowned;
        public Action<Draggable> OnEndedDrag;

        public void Dispose()
        {
            OnPointerDowned = null;
            OnEndedDrag = null;
        }
    }
}