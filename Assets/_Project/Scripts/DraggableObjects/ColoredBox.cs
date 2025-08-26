using _Project.Scripts.FileDatas;
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
        
        public new ColoredBoxData GetJsonData()
        {
            var baseData = base.GetJsonData();
            return new ColoredBoxData
            {
                TopColor = TopColor,
                BottomColor = BottomColor,
                AnchoredPosition = baseData.AnchoredPosition,
                AnchorMax = baseData.AnchorMax,
                AnchorMin = baseData.AnchorMin,
                ParentPath = baseData.ParentPath
            };
        }

        public void SetJsonData(ColoredBoxData data)
        {
            SetColor(data.TopColor, data.BottomColor);
            base.SetJsonData(data);
        }
    }
}