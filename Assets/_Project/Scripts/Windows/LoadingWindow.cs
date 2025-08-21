using _Project.Scripts.UIEffects;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Windows
{
    public class LoadingWindow : BaseWindow
    {
        [SerializeField] private RotateUIAroundPoint _rotateUIAroundPoint;

        public override Tween Show()
        {
            var sequence = DOTween.Sequence();
            sequence.AppendCallback(() => _rotateUIAroundPoint.StartRotation());
            sequence.Append(base.Show());
            return sequence;
        }
        
        public override Tween Hide()
        {
            var sequence = DOTween.Sequence();
            sequence.AppendCallback(() => _rotateUIAroundPoint.StopRotation());
            sequence.Append(base.Hide());
            return sequence;
        }
    }
}