using UnityEngine;

namespace _Project.Scripts.Helpers
{
    public static class RectTransformHelper
    {
        public static bool IsOverlappingAnchored(this RectTransform a, RectTransform b, float extraRadius = 0f)
        {
            var commonParent = a.root as RectTransform;

            Rect rectA = GetLocalRectInParent(a, commonParent, extraRadius);
            Rect rectB = GetLocalRectInParent(b, commonParent, extraRadius);

            return rectA.Overlaps(rectB);
        }

        public static Rect GetLocalRectInParent(RectTransform rectTransform, RectTransform parent, float extraRadius)
        {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            for (int i = 0; i < 4; i++)
                corners[i] = parent.InverseTransformPoint(corners[i]);

            Vector3 bottomLeft = corners[0];
            Vector3 topRight   = corners[2];

            Rect rect = new Rect(bottomLeft, topRight - bottomLeft);

            rect.xMin -= extraRadius;
            rect.yMin -= extraRadius;
            rect.xMax += extraRadius;
            rect.yMax += extraRadius;

            return rect;
        }
        
        public static string GetFullPath(this Transform transform)
        {
            if (transform == null) return null;
            string path = transform.name;
            while (transform.parent != null)
            {
                transform = transform.parent;
                path = transform.name + "/" + path;
            }
            return path;
        }
    }
}