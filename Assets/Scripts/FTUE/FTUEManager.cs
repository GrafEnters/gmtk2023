using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace {
    public class FTUEManager : MonoBehaviour {

        public Player Player;
        public MainAntagonist Hero;
        public UIManager UIManager;

        public NextLevelTrigger _leftDoor;
        public NextLevelTrigger _rightDoor;

        private bool _movingTutorActivated;
        private List<KeyCode> _moveControlsUsed = new List<KeyCode>();
        private List<KeyCode> _moveControls = new List<KeyCode>() {
            KeyCode.W,
            KeyCode.A,
            KeyCode.S,
            KeyCode.D
        };

        private static bool isStarted;

        public static FTUEManager Instance;
        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            
            if (SaveDataManager.Data.IsTutor2Passed) {
                _leftDoor.SetOpened();
                _rightDoor.SetOpened();
            }

            if (SaveDataManager.Data.IsTutor1Passed && !isStarted) {
                Hero.gameObject.SetActive(true);
            } else {
                Hero.gameObject.SetActive(false);
                StartCoroutine(ShowScenario());
            }
            
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;

            isStarted = true;
        }

        private void OnActiveSceneChanged(Scene prev, Scene next) {
            if (next.name.Contains("Gnome")) {
                Gnome[] objects = GetComponentsInRootObjects<Gnome>(next);
                if (objects.Length > 0) {

                    if (!SaveDataManager.Data.IsTutor2Passed) {
                        foreach (Gnome gnome in objects) {
                            gnome.OnStun += delegate {
                                SaveDataManager.Data.IsTutor2Passed = true;
                                EntryPoint.SaveDataManager.Save();
                                UIManager.HideBottomText();
                            };
                        }
                    
                        UIManager.ShowBottomText("PRESS LEFT MOUSE BUTTON", Color.white);
                    } else if (!SaveDataManager.Data.IsTutor3Passed) {
                        foreach (Gnome gnome in objects) {
                            gnome.OnUnderControl += delegate {
                                SaveDataManager.Data.IsTutor3Passed = true;
                                EntryPoint.SaveDataManager.Save();
                                UIManager.HideBottomText();
                            };
                        }
                    
                        UIManager.ShowBottomText("PRESS RIGHT MOUSE BUTTON", Color.white);
                    }
                }
            }
        }
        
        public static T[] GetComponentsInRootObjects<T>(Scene scene)
        {
            GameObject[] rootObjects = scene.GetRootGameObjects();
            var components = new List<T>();

            for (int i = 0; i < rootObjects.Length; i++)
            {
                GetComponentsInChildren(rootObjects[i].transform, components);
            }

            return components.ToArray();
        }

        private static void GetComponentsInChildren<T>(Transform transform, List<T> components)
        {
            T component = transform.GetComponent<T>();
            if (component != null)
            {
                components.Add(component);
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                GetComponentsInChildren(transform.GetChild(i), components);
            }
        }

        private IEnumerator ShowScenario() {
            yield return ShowScene1();
            yield return ShowScene2();
            yield return ShowScene3();
            
            if (SaveDataManager.Data != null) {
                SaveDataManager.Data.IsTutor1Passed = true;
                EntryPoint.SaveDataManager.Save();
            }
        }

        private IEnumerator ShowScene1() {
            Player.Spine.ShowSpineAnimation("Jump");
            Player.Animation.Play("Appear");
            
            do {
                yield return null;
            } while (Player.Animation.IsPlaying("Appear"));
            
            Player.Spine.ShowSpineAnimation("Landing");
        }
        
        private IEnumerator ShowScene2() {
            yield return new WaitForSeconds(1f);
            _movingTutorActivated = true;
            UIManager.SetWASDVisibility(true);
            yield return new WaitUntil(() => !_movingTutorActivated);
            UIManager.SetWASDVisibility(false);
        }

        private IEnumerator ShowScene3() {
            Hero.gameObject.SetActive(true);
            Hero.SetSpeakMode(true);
            Hero.SetDefendingMode(false);
            Hero.Animation.Play("Appear");
            do {
                yield return null;
            } while (Hero.Animation.IsPlaying("Appear"));

            Hero.PopUp.Show("Im gonna llik you! Hahah ah!");
            yield return new WaitForSeconds(2f);
            Hero.PopUp.Show("Paw pau!");
            yield return new WaitForSeconds(1f);
            Hero.PopUp.Show("RUUUN!!");
            Hero.SetSpeakMode(false);
            yield return new WaitForSeconds(3f);
            Hero.PopUp.Hide();
        }

        private void Update() {
            if (Input.anyKeyDown) {
                if (_movingTutorActivated) {
                    foreach (KeyCode keyCode in _moveControls) {
                        if (Input.GetKeyDown(keyCode)) {
                            if (!_moveControlsUsed.Contains(keyCode)) {
                                _moveControlsUsed.Add(keyCode);
                            }
                        }
                    }

                    if (_moveControlsUsed.Count == _moveControls.Count) {
                        _movingTutorActivated = false;
                    }
                }
            }
        }
    }
}