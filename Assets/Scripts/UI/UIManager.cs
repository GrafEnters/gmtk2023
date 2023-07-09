using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace {
    public class UIManager : MonoBehaviour {
        public static UIManager Instance;

        [SerializeField]
        private Sprite _demonSkill, _gnomeSkill, _elfSkill, _fairySkill, _inSkill, _outSkill;

        [SerializeField]
        private Image _lkm, _pkm;

        [SerializeField]
        private TextMeshProUGUI _bottomTextField;

        [SerializeField]
        private GameObject _wasd;
        
        private Controllable _curControllable;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        public void ShowBottomText(string text, Color color) {
            _bottomTextField.gameObject.SetActive(true);
            _bottomTextField.text = text;
            _bottomTextField.color = color;
        }

        public void HideBottomText() {
            _bottomTextField.gameObject.SetActive(false);
            _bottomTextField.color = Color.white;
            _bottomTextField.text = "";
        }

        public void SetWASDVisibility(bool visible) {
            _wasd.SetActive(visible);
        }

        private void Update() {
            if (_curControllable != Controllable.CurrentUnderControl) {
                UpdateSkillsIcons();
            }
        }

        private void UpdateSkillsIcons() {
            if (Controllable.CurrentUnderControl is Player) {
                _lkm.sprite = _demonSkill;
                _pkm.sprite = _inSkill;
            } else {
                _pkm.sprite = _outSkill;
            }

            if (Controllable.CurrentUnderControl is Gnome || Controllable.CurrentUnderControl is BigGnome) {
                _lkm.sprite = _gnomeSkill;
            }

            if (Controllable.CurrentUnderControl is Elf || Controllable.CurrentUnderControl is BigElf) {
                _lkm.sprite = _elfSkill;
            }

            if (Controllable.CurrentUnderControl is Fairy || Controllable.CurrentUnderControl is BigFairy) {
                _lkm.sprite = _fairySkill;
            }
        }
    }
}