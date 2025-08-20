using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Rendering
{
    [RequireComponent(typeof(Graphic))]
    public class Gradient : BaseMeshEffect
    {
        public Color _topColor = Color.white;
        public Color _bottomColor = Color.black;

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive() || vh.currentVertCount == 0) return;

            UIVertex vert = new UIVertex();
            int count = vh.currentVertCount;

            for (int i = 0; i < count; i++)
            {
                vh.PopulateUIVertex(ref vert, i);
                float lerp = vert.position.y / ((RectTransform)transform).rect.height;
                vert.color = Color.Lerp(_bottomColor, _topColor, lerp + 0.5f);
                vh.SetUIVertex(vert, i);
            }
        }
    }
}