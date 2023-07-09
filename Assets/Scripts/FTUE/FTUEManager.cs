using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace {
    public class FTUEManager : MonoBehaviour {

        public Player Player;
        public MainAntagonist Hero;
        public UIManager UIManager;

        private bool _movingTutorActivated;
        private List<KeyCode> _moveControlsUsed = new List<KeyCode>();
        private List<KeyCode> _moveControls = new List<KeyCode>() {
            KeyCode.W,
            KeyCode.A,
            KeyCode.S,
            KeyCode.D
        };

        private static bool isStarted;
        
        private void Awake() {
            bool IsFTUEPassed = false;
            if (SaveDataManager.Data != null) {
                IsFTUEPassed = SaveDataManager.Data.IsFTUEPassed;
            }
            if (IsFTUEPassed && !isStarted) {
                Hero.gameObject.SetActive(true);
            } else {
                Hero.gameObject.SetActive(false);
            }

            isStarted = true;

            StartCoroutine(ShowScenario());
        }

        private IEnumerator ShowScenario() {
            yield return ShowScene1();
            yield return ShowScene2();
            yield return ShowScene3();
            
            if (SaveDataManager.Data != null) {
                SaveDataManager.Data.IsFTUEPassed = true;
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
            Hero.Animation.Play("Appear");
            do {
                yield return null;
            } while (Hero.Animation.IsPlaying("Appear"));

            Hero.PopUp.Show("Im gonna llik you! Hahah ah!");
            yield return new WaitForSeconds(2f);
            Hero.PopUp.Show("Paw pau!");
            yield return new WaitForSeconds(1f);
            Hero.PopUp.Show("RUUUN!!");
            yield return new WaitForSeconds(0.3f);

            Hero.SetSpeakMode(false);
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