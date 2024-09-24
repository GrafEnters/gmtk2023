using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            } else {
                if (Instance != this) {
                    Destroy(this.gameObject);
                }
            }

            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private void OnActiveSceneChanged(Scene prev, Scene next) {
            if (next.name == "Menu") {
                MainAntagonist[] antagonist = GetComponentsInRootObjects<MainAntagonist>(next);
                if (antagonist.Length > 0) {
                    Hero = antagonist.First();
                }

                if (SaveDataManager.Data.IsTutor1Passed) {
                    Hero.gameObject.SetActive(true);
                } else {
                    Player.IsLockedMovement = true;
                    Hero.gameObject.SetActive(false);
                    StartCoroutine(ShowScenario());
                }

                isStarted = true;

                if (SaveDataManager.Data.IsTutor2Passed) {
                    _leftDoor.SetOpened();
                    _rightDoor.SetOpened();
                }

                NextLevelTrigger[] tr = GetComponentsInRootObjects<NextLevelTrigger>(next);
                if (SaveDataManager.Data.IsTutor2Passed) {
                    foreach (var UPPER in tr) {
                        UPPER.OpenDoor();
                    }
                }
            }

            UIManager.HideBottomText();

            if (next.name.Contains("Gnome")) {
                Gnome[] objects = GetComponentsInRootObjects<Gnome>(next);
                if (objects.Length > 0) {
                    SaveDataManager.Data.IsTutor2Passed = true;
                    EntryPoint.SaveDataManager.Save();
                    if (!SaveDataManager.Data.IsTutor2Passed) {
                        foreach (Gnome gnome in objects) {
                            gnome.OnStun += delegate { UIManager.HideBottomText(); };
                        }

                        UIManager.ShowBottomText("USE LMB TO PUSH BACK ENEMIES", Color.white);
                    } else if (!SaveDataManager.Data.IsTutor3Passed) {
                        SaveDataManager.Data.IsTutor3Passed = true;
                        EntryPoint.SaveDataManager.Save();
                        foreach (Gnome gnome in objects) {
                            gnome.OnUnderControl += delegate { UIManager.HideBottomText(); };
                        }

                        UIManager.ShowBottomText("USE RMB TO POSSESS SOMEONE OR LEAVE A POSSESSED BODY", Color.white);
                    }
                }
            }

            if (next.name != "Menu") {
                CarryableObject[] objects = GetComponentsInRootObjects<CarryableObject>(next);
                if (objects.Length > 0) {
                    bool carry = objects.Any(o => o.GetComponentInChildren<Player>() == null);
                    if (carry) {
                        UIManager.ShowBottomText("PRESS 'E' TO LIFT TV PART", Color.white);
                    }
                }
            }
        }

        public static T[] GetComponentsInRootObjects<T>(Scene scene) {
            GameObject[] rootObjects = scene.GetRootGameObjects();
            var components = new List<T>();

            for (int i = 0; i < rootObjects.Length; i++) {
                GetComponentsInChildren(rootObjects[i].transform, components);
            }

            return components.ToArray();
        }

        private static void GetComponentsInChildren<T>(Transform transform, List<T> components) {
            T component = transform.GetComponent<T>();
            if (component != null) {
                components.Add(component);
            }

            for (int i = 0; i < transform.childCount; i++) {
                GetComponentsInChildren(transform.GetChild(i), components);
            }
        }

        private IEnumerator ShowScenario() {
            yield return ShowScene1();
            yield return ShowScene2();
            yield return ShowScene3();
            yield return ShowScene4();
        }

        private IEnumerator ShowScene1() {
            Player.IsLockedMovement = true;
            Player.Spine.SetAnimation("Jump", true);
            Player.Animation.Play("Appear");

            do {
                yield return null;
            } while (Player.Animation.IsPlaying("Appear"));

            Player.Spine.SetAnimation("idle", true);
        }

        private IEnumerator ShowScene2() {
            Player.IsLockedMovement = true;
            Player.PopUp.Show("Oh no! My TV is broken...");
            yield return new WaitForSeconds(2f);
            Player.IsLockedMovement = true;
            Player.PopUp.Show("I need to find all parts before\nI can get back home!");

            if (SaveDataManager.Data != null) {
                SaveDataManager.Data.IsTutor1Passed = true;
                EntryPoint.SaveDataManager.Save();
            }
        }

        private IEnumerator ShowScene3() {
            yield return new WaitForSeconds(2f);
            _movingTutorActivated = true;
            Player.IsLockedMovement = false;
            Player.PopUp.Hide();
            UIManager.SetWASDVisibility(true);
            yield return new WaitUntil(() => !_movingTutorActivated);
            UIManager.SetWASDVisibility(false);
        }

        private IEnumerator ShowScene4() {
            Hero.SetSpeakMode(true);
            Hero.gameObject.SetActive(true);
            Hero.SetDefendingMode(false);
            // Hero.Spine.SetAnimation("Jump", true);
            Hero.Animation.Play("Appear");

            do {
                yield return null;
            } while (Hero.Animation.IsPlaying("Appear"));

            Hero.Spine.SetAnimation("idle", true);

            Hero.PopUp.Show("Don't run! It's pointless!");
            yield return new WaitForSeconds(2f);
            Hero.PopUp.Show("I will catch you\nand then destroy you!");
            yield return new WaitForSeconds(1f);
            Hero.PopUp.Show("R-U-U-U-N!!!");
            Hero.SetSpeakMode(false);
            yield return new WaitForSeconds(3f);
            Hero.PopUp.Hide();
        }

        private void Update() {
            if (Input.anyKeyDown) {
                if (_movingTutorActivated) {
                    foreach (KeyCode keyCode in _moveControls) {
                        if (Input.GetKeyDown(keyCode)) {
                            _movingTutorActivated = false;
                            return;
                        }
                    }
                }
            }
        }
    }
}