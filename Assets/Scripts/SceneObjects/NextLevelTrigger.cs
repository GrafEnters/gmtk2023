using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using ZhukovskyGamesPlugin;

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

        if (!other.attachedRigidbody) {
            return;
        }

        if (other.attachedRigidbody.GetComponent<Controllable>()) {
            if (Controllable.CurrentUnderControl == other.attachedRigidbody.GetComponent<Controllable>() &&
                !(Controllable.CurrentUnderControl is Player)) {
                Controllable.CurrentUnderControl.FreeControllable();
                return;
            }

            _onTriggered = true;
            Player.Instance.gameObject.SetActive(false);
            DontDestroyOnLoad(Player.Instance.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(sceneNameToLoad);
            EntryPoint.Audio.PlaySound(Sounds.enter_door);
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

    public void CloseDoor() { }

    public void SetOpened() {
        try {
            _animation.Play("Opened");
        }
        catch { }
      
        _obstacle.enabled = false;
        _collider.isTrigger = true;
    }
}