using _Project.Scripts.Rendering;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Scripts.Windows
{
    public class GameWindow : BaseWindow
    {
        [SerializeField] private StarSpawner _starSpawner;
        [SerializeField] private LineSpawner _lineSpawner;

        public override void Initialize()
        {
            _lineSpawner.FillBg();
            _starSpawner.FillBg();
        }
    }
}