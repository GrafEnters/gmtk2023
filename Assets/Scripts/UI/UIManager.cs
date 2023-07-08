using TMPro;
using UnityEngine;

namespace DefaultNamespace {
    public class UIManager : MonoBehaviour {
        public static UIManager Instance;
        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        [SerializeField]
        private TextMeshProUGUI _bottomTextField;

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
    }
}