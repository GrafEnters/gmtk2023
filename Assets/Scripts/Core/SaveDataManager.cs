using ZhukovskyGamesPlugin;

namespace DefaultNamespace {
    public class SaveDataManager {
        private const string dataPath = "GAME_DATA";
        public static GameData Data = new GameData();

        public void Load() {
            Data = JsonPrefsSaveLoadManager.Load<GameData>(dataPath);
            if (Data == null) {
                Data = new GameData();
            }
        }

        public void Save() {
            JsonPrefsSaveLoadManager.Save(Data, dataPath);
        }
    }
}