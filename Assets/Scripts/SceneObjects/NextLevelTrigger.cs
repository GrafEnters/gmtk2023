using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour {
    [SerializeField]
    private string sceneNameToLoad;
    [SerializeField]
    private Animation _animation;
    [SerializeField]
    private Collider _collider;
    [SerializeField]
    private NavMeshObstacle _obstacle;

    public bool IsOpenedByDefault;
    
    private bool _onTriggered;

    [SerializeField]
    private bool IsDisablingCubesOnAwake = false;

    private void Start() {
        if (IsOpenedByDefault) {
            SetOpened();
        }
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

    public void OpenDoor() {
        _animation.Play("Open");
        _obstacle.enabled = false;
        _collider.isTrigger = true;
    }

    public void CloseDoor() {
        
    }

    public void SetOpened() {
        _animation.Play("Opened");
        _obstacle.enabled = false;
        _collider.isTrigger = true;
    }
}
