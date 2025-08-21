using _Project.Scripts.DraggableObjects;
using UnityEngine;

namespace _Project.Scripts.SO
{
    [CreateAssetMenu(fileName = "ColoredBoxesConfig", menuName = "SO/Colored Boxes Config")]
    public class ColoredBoxesConfig : ScriptableObject
    {
        [field:SerializeField] public ColoredBox BoxRed { get; private set; }
        [field:SerializeField] public ColoredBox BoxLightRed { get; private set; }
        [field:SerializeField] public ColoredBox BoxOrange { get; private set; }
        [field:SerializeField] public ColoredBox BoxYellow { get; private set; }
        [field:SerializeField] public ColoredBox BoxGreen { get; private set; }
        [field:SerializeField] public ColoredBox BoxBlue { get; private set; }
        [field:SerializeField] public ColoredBox BoxDarkBlue { get; private set; }
        [field:SerializeField] public ColoredBox BoxPurple { get; private set; }
        [field:SerializeField] public ColoredBox BoxGray { get; private set; }
        [field:SerializeField] public ColoredBox BoxBrown { get; private set; }
        [field:SerializeField] public ColoredBox BoxBlack { get; private set; }
        [field:SerializeField] public ColoredBox BoxDarkPurple { get; private set; }
        [field:SerializeField] public ColoredBox BoxPink { get; private set; }
        [field:SerializeField] public ColoredBox BoxTurquoise { get; private set; }
        [field:SerializeField] public ColoredBox BoxLightGreen { get; private set; }
        [field:SerializeField] public ColoredBox BoxLilac { get; private set; }
        [field:SerializeField] public ColoredBox BoxLightLilac { get; private set; }
        [field:SerializeField] public ColoredBox BoxBrownRed { get; private set; }
        [field:SerializeField] public ColoredBox BoxLightYellow { get; private set; }
        [field:SerializeField] public ColoredBox BoxLightPink { get; private set; }
    }
}