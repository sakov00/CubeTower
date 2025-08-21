using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Rendering
{
    public class StarSpawner : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private RectTransform _spawnArea;
        [SerializeField] private List<RectTransform> _largeStars;
        [SerializeField] private List<RectTransform> _mediumStars;
        [SerializeField] private List<RectTransform> _smallStars;
        [SerializeField] private int _maxAttemptsPerObject = 50;
        [SerializeField] private float _extraSpacing = 10f;
        [SerializeField] private float _xJitter = 30f;
        [SerializeField] private bool _isRotate;

        private readonly List<Rect> _placedRects = new();

        public void FillBg()
        {
            SpawnCategory(_largeStars);
            SpawnCategory(_mediumStars);
            SpawnCategory(_smallStars);
        }

        private void SpawnCategory(List<RectTransform> starPrefabs)
        {
            var y = -_spawnArea.rect.height / 2;
            var canPlaceMore = true;

            while (y < _spawnArea.rect.height / 2 && canPlaceMore)
            {
                canPlaceMore = false;
                var prefab = starPrefabs[Random.Range(0, starPrefabs.Count)];

                for (var attempt = 0; attempt < _maxAttemptsPerObject; attempt++)
                {
                    var rectTransform = Instantiate(prefab, _spawnArea);
                    var size = rectTransform.sizeDelta;

                    var x = Random.Range(-_spawnArea.rect.width / 2, _spawnArea.rect.width / 2);
                    x += Random.Range(-_xJitter, _xJitter);

                    var pos = new Vector2(x, y);

                    var newRect = new Rect(pos - size / 2, size);
                    var overlaps = _placedRects.Any(existingRect => existingRect.Overlaps(newRect));

                    if (!overlaps)
                    {
                        rectTransform.anchoredPosition = pos;

                        if (_isRotate)
                            rectTransform.localRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

                        _placedRects.Add(newRect);
                        canPlaceMore = true;

                        y += size.y + _extraSpacing;
                        break;
                    }
                    else
                    {
                        Destroy(rectTransform.gameObject);
                    }
                }

                if (!canPlaceMore)
                {
                    y += _extraSpacing;
                }
            }
        }
    }
}
