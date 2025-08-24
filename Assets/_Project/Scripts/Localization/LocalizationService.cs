using System.Collections.Generic;
using System.IO;
using UniRx;
using UnityEngine;
using Newtonsoft.Json;

namespace _Project.Scripts.Localization
{
    public class LocalizationService
    {
        private readonly Dictionary<string, Dictionary<string, string>> _localizedTexts = new();
        
        private const string LocalizationFileName = "localization.json";

        public readonly ReactiveProperty<string> CurrentLanguage = new("En");
        
        public LocalizationService()
        {
            LoadFromStreamingAssets();
        }

        private void LoadFromStreamingAssets()
        {
            var path = Path.Combine(Application.streamingAssetsPath, LocalizationFileName);

            if (!File.Exists(path))
                return;

            try
            {
                var json = File.ReadAllText(path);
                var items = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);

                foreach (var item in items)
                {
                    if (!item.TryGetValue("Id", out var id))
                    {
                        continue;
                    }

                    var dict = new Dictionary<string, string>(item);
                    dict.Remove("Id");
                    _localizedTexts[id] = dict;
                }
            }
            catch (JsonException e)
            {
                Debug.LogError($"Failed to parse localization JSON: {e}");
            }
        }
        
        public string GetText(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "[MISSING]";
            
            if (_localizedTexts.TryGetValue(key, out var dict) && dict.TryGetValue(CurrentLanguage.Value, out var text))
                return text;
            
            return key;
        }
    }
}