using System.Collections.Generic;
using System.Linq;
using _Project.Scripts._VContainer;
using _Project.Scripts.DraggableObjects;
using _Project.Scripts.Factories;
using _Project.Scripts.Managers;
using _Project.Scripts.Windows;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.ObjectPools
{
    public class DraggablePool : MonoBehaviour
    {
        [Inject] private DraggableFactory _draggableFactory;
        
        private readonly List<Draggable> _availableDraggables = new();

        private void Awake()
        {
            InjectManager.Inject(this);
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
            return (T)draggable;
        }

        public void Return(Draggable box)
        {
            if (!_availableDraggables.Contains(box))
            {
                _availableDraggables.Add(box);
            }
            
            box.gameObject.SetActive(false);
            box.transform.SetParent(transform, false); 
        }
    }
}