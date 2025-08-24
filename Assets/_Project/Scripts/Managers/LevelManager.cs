using System;
using System.Collections.Generic;
using System.IO;
using _Project.Scripts.DraggableObjects;
using _Project.Scripts.Factories;
using _Project.Scripts.Registries;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.Managers
{
    public class LevelManager
    {
        [Inject] private SaveRegistry _saveRegistry;
        [Inject] private DraggableFactory _draggableFactory;

        private string SavePath => Path.Combine(Application.persistentDataPath, "progress.json");

        public void SaveLevel()
        {
            var data = new LevelData();

            foreach (var draggableObj in _saveRegistry.GetAll<ColoredBox>())
            {
                data.Objects.Add(draggableObj.GetJsonData());
            }

            var json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
        }

        public void LoadLevel()
        {
            if (!File.Exists(SavePath))
                return;

            var json = File.ReadAllText(SavePath);
            var data = JsonUtility.FromJson<LevelData>(json);

            foreach (var draggableObj in data.Objects)
            {
                if (draggableObj.GetType() == typeof(ColoredBoxData))
                {
                    if (!string.IsNullOrEmpty(draggableObj.ParentPath))
                    {
                        var parent = GameObject.Find(draggableObj.ParentPath)?.transform;
                        if (parent != null)
                        {
                            var coloredBox = _draggableFactory.CreateDraggable<ColoredBox>(parent);
                            coloredBox.SetJsonData(draggableObj);
                        }
                    }

                }

            }
        }
    }

    [Serializable]
    public class LevelData
    {
        public List<ColoredBoxData> Objects = new();
    }
    
    [Serializable]
    public class DraggableData
    {
        public Vector3 AnchoredPosition;
        public Vector3 AnchorMax;
        public Vector3 AnchorMin;
        public string ParentPath;
    }

    [Serializable]
    public class ColoredBoxData : DraggableData
    {
        public Color TopColor;
        public Color BottomColor;
    }
}
