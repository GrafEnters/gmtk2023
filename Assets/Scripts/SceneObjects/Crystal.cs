using UnityEngine;
using UnityEngine.Events;

public class Crystal : MonoBehaviour {
    [SerializeField]
    public UnityEvent _onTeleported;

    [SerializeField]
    private bool isDestroyOnTeleport = true;

    public void OnTeleportedByPlayer() { }

    public void OnTeleportedByBigPlayer() {
        _onTeleported?.Invoke();
        if (isDestroyOnTeleport) {
            Destroy(gameObject);
        }
    }
}