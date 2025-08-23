using System;
using System.Collections.Generic;
using _Project.Scripts._VContainer;
using _Project.Scripts.DraggableObjects;
using _Project.Scripts.Factories;
using _Project.Scripts.Managers;
using _Project.Scripts.ObjectPools;
using _Project.Scripts.Rendering;
using _Project.Scripts.Zones;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.Windows
{
    public class GameWindow : BaseWindow
    {
        [SerializeField] private List<BaseZone> _zones = new();
        [SerializeField] private StarSpawner _starSpawner;
        [SerializeField] private LineSpawner _lineSpawner;
        [SerializeField] private RectTransform _scrollContent;
        [SerializeField] private ColoredBoxesPool _coloredBoxesPool;
        
        [Inject] private DraggableManager _draggableManager;

        public override void Initialize()
        {
            _lineSpawner.FillBg();
            _starSpawner.FillBg();
            _draggableManager.OnPointerDowned += CopyCube;
            _draggableManager.OnEndedDrag += CubeReplaced;
            _coloredBoxesPool.Ininitialize(_scrollContent, this);
        }

        private void CubeReplaced(Draggable draggable)
        {
            foreach (var zone in _zones)
            {
                var screenPoint = RectTransformUtility.WorldToScreenPoint(null, draggable.RectTransform.position);
                if (RectTransformUtility.RectangleContainsScreenPoint(zone.RectTransform, screenPoint))
                {
                    if(zone.RectTransform)
                        zone.AddDraggableToZone(draggable);
                }
            }
        }

        private void CopyCube(Draggable draggable)
        {
            if (draggable is ColoredBox coloredBox && _scrollContent.transform == draggable.RectTransform.parent)
            {
                _coloredBoxesPool.Get(_scrollContent, this, coloredBox.TopColor, coloredBox.BottomColor);
            }
        }

        private void OnDestroy()
        {
            _draggableManager.OnPointerDowned -= CopyCube;
            _draggableManager.OnEndedDrag -= CubeReplaced;
        }
    }
}