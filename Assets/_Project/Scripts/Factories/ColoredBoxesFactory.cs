using System.Collections.Generic;
using _Project.Scripts.DraggableObjects;
using _Project.Scripts.SO;
using _Project.Scripts.Windows;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Factories
{
    public class ColoredBoxesFactory
    {
        [Inject] private IObjectResolver _resolver;
        [Inject] private ColoredBoxesConfig _coloredBoxesConfig;

        public List<ColoredBox> CreateAllColoredBoxes(Transform parent, BaseWindow parentWindow, Vector3 position = default, Quaternion rotation= default)
        {
            var coloredBoxes = new List<ColoredBox>();
            foreach (var colors in _coloredBoxesConfig.ColoredBoxes)
            {
                var coloredBox = _resolver.Instantiate(_coloredBoxesConfig.BoxPrefab, position, rotation, parent);
                coloredBox.Initialize(parentWindow);
                coloredBox.SetColor(colors.TopColor, colors.BottomColor);
                coloredBoxes.Add(coloredBox);
            }
            return coloredBoxes;
        }
        
        public ColoredBox CreateColoredBox(Transform parent, BaseWindow parentWindow, Color topColor, Color bottomColor, Vector3 position = default, Quaternion rotation= default)
        {
            var coloredBox = _resolver.Instantiate(_coloredBoxesConfig.BoxPrefab, position, rotation, parent);
            coloredBox.Initialize(parentWindow);
            coloredBox.SetColor(topColor, bottomColor);
            return coloredBox;
        }
    }
}