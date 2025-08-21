using UnityEngine;

namespace _Project.Scripts.Rendering
{
    public class LineSpawner : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private RectTransform _spawnArea;
        [SerializeField] private RectTransform _linePrefab;
        [SerializeField] private float _lineSpacingX = 100f;
        [SerializeField] private float _lineSpacingY = 50f;

        public void FillBg()
        {
            var lineSize = _linePrefab.sizeDelta;

            var startY = -_spawnArea.rect.height / 2;
            var endY = _spawnArea.rect.height / 2;
            var startX = -_spawnArea.rect.width / 2;
            var endX = _spawnArea.rect.width / 2;

            var rowCount = Mathf.FloorToInt((_spawnArea.rect.height + _lineSpacingY) / _lineSpacingY);
            var colCount = Mathf.FloorToInt((_spawnArea.rect.width + _lineSpacingX) / _lineSpacingX);

            if (startY + rowCount * _lineSpacingY + lineSize.y / 2 <= endY) rowCount++;
            if (startX + colCount * _lineSpacingX + lineSize.x / 2 <= endX) colCount++;

            for (var row = 0; row < rowCount; row++)
            {
                var currentY = startY + row * _lineSpacingY + lineSize.y / 2;
                for (var col = 0; col < colCount; col++)
                {
                    var currentX = startX + col * _lineSpacingX + lineSize.x / 2;
                    var rectTransform = Instantiate(_linePrefab, _spawnArea);
                    rectTransform.anchoredPosition = new Vector2(currentX, currentY);
                }
            }
        }
    }
}