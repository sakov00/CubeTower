using _Project.Scripts.Rendering;
using UnityEngine;

namespace _Project.Scripts.DraggableObjects
{
    public class ColoredBox : Draggable
    {
        [SerializeField] private GradientImage _gradientImageBg;
        public Color TopColor => _gradientImageBg.TopColor;
        public Color BottomColor => _gradientImageBg.BottomColor;

        public void SetColor(Color topColor, Color bottomColor)
        {
            _gradientImageBg.TopColor = topColor;
            _gradientImageBg.BottomColor = bottomColor;
        }
    }
}