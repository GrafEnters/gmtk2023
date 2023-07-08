using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour {
    [SerializeField]
    private string sceneNameToLoad;
    [SerializeField]
    private GameObject disableOnAwake;
    private bool _onTriggered;

    private void Awake() {
        disableOnAwake.SetActive(false);
    }

    public void OnTriggerEnter(Collider other) {
        if (_onTriggered) {
            return;
        }

        if (other.CompareTag("Player")) {
            _onTriggered = true;
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }
}