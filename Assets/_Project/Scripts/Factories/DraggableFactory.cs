using System.Collections.Generic;
using _Project.Scripts.DraggableObjects;
using _Project.Scripts.Enums;
using _Project.Scripts.SO;
using _Project.Scripts.Windows;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Factories
{
    public class DraggableFactory
    {
        [Inject] private IObjectResolver _resolver;
        [Inject] private ColoredBoxesConfig _coloredBoxesConfig;

        public List<ColoredBox> CreateAllColoredBoxes(Transform parent)
        {
            var coloredBoxes = new List<ColoredBox>();
            foreach (var colors in _coloredBoxesConfig.ColoredBoxes)
            {
                var coloredBox = CreateDraggable<ColoredBox>(parent);
                coloredBox.SetColor(colors.TopColor, colors.BottomColor);
                coloredBoxes.Add(coloredBox);
            }
            return coloredBoxes;
        }
        
        public T CreateDraggable<T>(Transform parent) where T : Draggable
        {
            Draggable draggable = null;
            foreach (var prefab in _coloredBoxesConfig.ListDraggablePrefabs)
            {
                if (prefab is T tPrefab)
                {
                    draggable = _resolver.Instantiate(tPrefab, parent);
                    draggable.Initialize();
                    break;
                }
            }
            
            return (T)draggable;
        }
    }
}