using UnityEngine;
using UnityEngine.SceneManagement;
using ZhukovskyGamesPlugin;

namespace DefaultNamespace {
    public class EntryPoint : EntrySingleton {
        public static SaveDataManager SaveDataManager;

        public static EntryPoint Instance => instance as EntryPoint;

        [SerializeField]
        private Loading _loading;

        public void Awake() {
            if (!TryCreateSingleton()) {
                return;
            }

            SaveDataManager = new SaveDataManager();
            SaveDataManager.Load();

            _loading.SetProgress(0);
            _loading.StartFakeLoading(OnLoadingEnd);

            //TODO init all systems and loading screen, then launch game
        }

        private void OnLoadingEnd() {
            SceneManager.LoadScene(1);
        }
    }
}