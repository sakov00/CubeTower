using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.FileDatas
{
    public class LocalizationService
    {
        private readonly Dictionary<string, Dictionary<string, string>> _localizedTexts = new();

        private const string LocalizationFileName = "Localization.json";

        public readonly ReactiveProperty<string> CurrentLanguage = new("En");
        
        public async UniTask LoadAsync()
        {
            string json = await ReadLocalizationFileAsync();
            if (string.IsNullOrEmpty(json))
                return;

            ParseLocalization(json);
        }
        
        private async UniTask<string> ReadLocalizationFileAsync()
        {
            var path = Path.Combine(Application.streamingAssetsPath, LocalizationFileName);

#if UNITY_ANDROID && !UNITY_EDITOR
            using var request = UnityWebRequest.Get(path);
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[Localization] Failed to load file at {path}: {request.error}");
                return null;
            }

            return request.downloadHandler.text;
#else
            if (File.Exists(path)) 
                return await File.ReadAllTextAsync(path);

            return null;
#endif
        }

        private void ParseLocalization(string json)
        {
            try
            {
                var items = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);
                if (items == null)
                    return;

                foreach (var item in items)
                {
                    if (!item.TryGetValue("Id", out var id))
                        continue;

                    var dict = new Dictionary<string, string>(item);
                    dict.Remove("Id");
                    _localizedTexts[id] = dict;
                }
            }
            catch (JsonException e)
            {
                Debug.LogError($"[Localization] Failed to parse JSON: {e}");
            }
        }

        public string GetText(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "[MISSING]";

            if (_localizedTexts.TryGetValue(key, out var dict) &&
                dict.TryGetValue(CurrentLanguage.Value, out var text))
                return text;

            return key;
        }
    }
}
