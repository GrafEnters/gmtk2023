using UnityEngine;

namespace ZhukovskyGamesPlugin {
    public static class JsonPrefsSaveLoadManager {
        public static void Save(object obj, string Path) {
            string saveStr = JsonUtility.ToJson(obj);
            PlayerPrefs.SetString(Path, saveStr);
        }

        public static T Load<T>(string path) {
            string saveStr = PlayerPrefs.GetString(path, "");
            if (string.IsNullOrEmpty(saveStr)) {
                return default;
            }

            return JsonUtility.FromJson<T>(saveStr);
        }
    }
}