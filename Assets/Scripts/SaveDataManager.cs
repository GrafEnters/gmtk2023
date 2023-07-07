using UnityEngine;
using ZhukovskyGamesPlugin;

namespace DefaultNamespace {
    public class SaveDataManager {
        private const string dataPath = "GAME_DATA";
        public static GameData Data;

        public void Load() {
            Data = JsonPrefsSaveLoadManager.Load<GameData>(dataPath);
            if (Data == null) {
                Data = new GameData();
            }
        }

        public void Save() {
            GenerateRandomNumber();
            JsonPrefsSaveLoadManager.Save(Data, dataPath);
        }
        
        private void GenerateRandomNumber() {
            Data.RandomNumber = Random.Range(0,1001);
        }
    }
}