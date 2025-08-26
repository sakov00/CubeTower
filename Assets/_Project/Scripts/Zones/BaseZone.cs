using _Project.Scripts._VContainer;
using _Project.Scripts.Analytics;
using _Project.Scripts.DraggableObjects;
using _Project.Scripts.ObjectPools;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.Zones
{
    public abstract class BaseZone : MonoBehaviour
    {
        [field: SerializeField] public RectTransform RectTransform { get; private set; }
        
        [Inject] protected DraggablePool DraggablePool;
        [Inject] protected ActionNotifier ActionNotifier;

        private void OnValidate()
        {
            RectTransform ??= GetComponent<RectTransform>();
        }

        public virtual void Initialize()
        {
            InjectManager.Inject(this);
        }
        
        public virtual void RestoreData() { }
        public abstract UniTask AddDraggableToZone<T>(T draggable) where T : Draggable;
        public virtual void RemoveDraggableFromZone(Draggable draggable) { }
    }
}