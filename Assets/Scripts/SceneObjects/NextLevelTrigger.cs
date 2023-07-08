using System.Linq;
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
            Player.Instance.gameObject.SetActive(false);
            DontDestroyOnLoad(Player.Instance.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameObject availablePlayer = scene.GetRootGameObjects().FirstOrDefault(o => o.GetComponentInChildren<Player>());
        Player.Instance.gameObject.transform.SetParent(null);
        SceneManager.MoveGameObjectToScene(Player.Instance.gameObject, scene);
        if (availablePlayer != null) {
            Player.Instance.transform.position = availablePlayer.transform.position;
            Destroy(availablePlayer.gameObject);
        }
        Player.Instance.gameObject.SetActive(true);
    }
}
