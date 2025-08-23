using System;
using System.Collections.Generic;
using _Project.Scripts.DraggableObjects;
using UnityEngine;

namespace _Project.Scripts.SO
{
    [CreateAssetMenu(fileName = "ColoredBoxesConfig", menuName = "SO/Colored Boxes Config")]
    public class ColoredBoxesConfig : ScriptableObject
    {
        [field:SerializeField] public ColoredBox BoxPrefab { get; set; }
        [field:SerializeField] public List<ColorsConfig> ColoredBoxes { get; set; } = new();
    }
    
    [Serializable]
    public struct ColorsConfig
    {
        [field:SerializeField] public Color TopColor { get; set; }
        [field:SerializeField] public Color BottomColor { get; set; }
    }
}