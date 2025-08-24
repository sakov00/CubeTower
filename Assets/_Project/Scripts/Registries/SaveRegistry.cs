using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.DraggableObjects;

namespace _Project.Scripts.Registries
{
    public class SaveRegistry
    {
        private readonly Dictionary<Type, IList> _typedCollections = new();

        public void Register<T>(T draggable) where T : Draggable
        {
            var type = typeof(T);

            if (!_typedCollections.TryGetValue(type, out var list))
            {
                list = new List<T>();
                _typedCollections[type] = list;
            }

            var typedList = (List<T>)list;
            if (!typedList.Contains(draggable))
            {
                typedList.Add(draggable);
            }
        }

        public void Unregister<T>(T draggable) where T : Draggable
        {
            var type = typeof(T);
            if (_typedCollections.TryGetValue(type, out var list))
            {
                var typedList = (List<T>)list;
                if (typedList.Contains(draggable))
                {
                    typedList.Remove(draggable);
                }
            }
        }

        public List<T> GetAll<T>()
        {
            var type = typeof(T);

            if (_typedCollections.TryGetValue(type, out var listObj))
            {
                return (List<T>)listObj;
            }

            var newList = new List<T>();
            _typedCollections[type] = newList;
            return newList;
        }

        public void Clear()
        {
            _typedCollections.Clear();
        }
    }
}