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
    public class ColoredBoxesPool : MonoBehaviour
    {
        [Inject] private ColoredBoxesFactory _coloredBoxesFactory;
        
        private List<ColoredBox> _availableBoxes = new();

        private void Awake()
        {
            InjectManager.Inject(this);
        }

        public List<ColoredBox> Ininitialize(Transform parent, BaseWindow baseWindow)
        {
            var startedBoxes = _coloredBoxesFactory.CreateAllColoredBoxes(parent, baseWindow);
            return startedBoxes;
        }
        
        public ColoredBox Get(Transform parent, BaseWindow baseWindow, Color topColor, Color bottomColor)
        {
            var box = _availableBoxes.FirstOrDefault(x => x.TopColor == topColor && x.BottomColor == bottomColor);

            if (box != null)
            {
                _availableBoxes.Remove(box);
                box.gameObject.SetActive(true);
            }
            else
            {
                box = _coloredBoxesFactory.CreateColoredBox(parent, baseWindow, topColor, bottomColor);
            }
            
            box.transform.SetParent(parent, false);
            return box;
        }

        public void Return(ColoredBox box)
        {
            if (!_availableBoxes.Contains(box))
            {
                _availableBoxes.Add(box);
            }
            
            box.gameObject.SetActive(false);
            box.transform.SetParent(transform, false); 
        }
    }
}