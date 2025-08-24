using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.DraggableObjects;
using _Project.Scripts.Factories;
using _Project.Scripts.Registries;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.ObjectPools
{
    public class DraggablePool
    {
        [Inject] private DraggableFactory _draggableFactory;
        [Inject] private SaveRegistry _saveRegistry;
        
        private Transform _containerTransform;
        
        private readonly List<Draggable> _availableDraggables = new();

        public void SetContainer(Transform transform)
        {
            _containerTransform = transform;
        }

        public List<ColoredBox> CreateAllColoredBoxes(Transform parent)
        {
            return _draggableFactory.CreateAllColoredBoxes(parent);
        }
        
        public T Get<T>(Transform parent) where T : Draggable
        {
            var draggable = _availableDraggables.OfType<T>().FirstOrDefault();

            if (draggable != null)
            {
                _availableDraggables.Remove(draggable);
                draggable.gameObject.SetActive(true);
            }
            else
            {
                draggable = _draggableFactory.CreateDraggable<T>(parent);
            }
            
            draggable.transform.SetParent(parent, false);
            _saveRegistry.Register<T>(draggable);
            return draggable;
        }

        public void Return<T>(T draggable) where T : Draggable
        {
            if (!_availableDraggables.Contains(draggable))
            {
                _availableDraggables.Add(draggable);
            }
            
            draggable.gameObject.SetActive(false);
            draggable.transform.SetParent(_containerTransform, false); 
            _saveRegistry.Unregister<T>(draggable);
        }
    }
}