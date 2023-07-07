using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZhukovskyGamesPlugin {
    public class EntrySingleton : MonoBehaviour{

        protected static EntrySingleton instance;
        
        protected bool TryCreateSingleton() {
            if (instance == null) {
                if (SceneManager.GetActiveScene().buildIndex == 0) {
                    CreateSingleton();
                    return true;
                }

                SceneManager.LoadScene(0);
            }

            Destroy(gameObject);
            return false;
        }

        private void CreateSingleton() {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}