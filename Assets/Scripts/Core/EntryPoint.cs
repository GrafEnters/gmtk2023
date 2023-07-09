using UnityEngine;
using UnityEngine.SceneManagement;
using ZhukovskyGamesPlugin;

namespace DefaultNamespace {
    public class EntryPoint : EntrySingleton {
        public static SaveDataManager SaveDataManager;
        public static Audio Audio;

        [SerializeField]
        private Audio _audio;

        public static EntryPoint Instance => instance as EntryPoint;

        [SerializeField]
        private Loading _loading;
        [SerializeField]
        private InputHeroName _inputHeroName;

        [SerializeField]
        private Comics _comics;

        [SerializeField]
        private bool IsEraseDataEveryTime = true;

        public void Awake() {
            if (!TryCreateSingleton()) {
                return;
            }

            if (IsEraseDataEveryTime) {
                PlayerPrefs.DeleteAll();
            }

            SaveDataManager = new SaveDataManager();
            SaveDataManager.Load();

            Audio = _audio;
            _audio.Init();
            if (!string.IsNullOrEmpty(SaveDataManager.Data.HeroName)) {
                SceneManager.LoadScene(1);
                return;
            }

            //_loading.SetProgress(0);
            //_loading.StartFakeLoading(null);
            _inputHeroName.StartCoroutine(_inputHeroName.WaitForHeroName(_comics.Init));


            //TODO init all systems and loading screen, then launch game
        }

        private void OnLoadingEnd() {
            SceneManager.LoadScene(1);
        }
    }
}