using TMPro;
using UnityEngine;

namespace DefaultNamespace {
    public class SpeechPopUp : MonoBehaviour {
        public TextMeshPro Text;
        public void Show(string text) {
            gameObject.SetActive(true);
            Text.text = text;
        }
    }
}