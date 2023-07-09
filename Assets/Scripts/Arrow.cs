using UnityEngine;

public class Arrow : MonoBehaviour {
    [SerializeField]
    private Rigidbody _rb;

    private Vector3 _dir;
    private float _speed;
    private bool _isPlayerShot;
    private Controllable _enemy;

    public void SetFlyDirection(Vector3 dir, float speed, bool isPlayer, Controllable enemy) {
        _enemy = enemy;
        SetFlyDirection(dir, speed, isPlayer);
    }
    
    public void SetFlyDirection(Vector3 dir, float speed, bool isPlayer) {
        Destroy(gameObject, 5f);
        _isPlayerShot = isPlayer;
        _speed = speed;
        _dir.y = 0;
        _dir = dir;
        transform.rotation = Quaternion.LookRotation(_dir, Vector3.up);
    }

    private void FixedUpdate() {
        _rb.MovePosition(_rb.position + _dir.normalized * _speed);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.attachedRigidbody.GetComponent<Aim>() != null && _isPlayerShot) {
            other.attachedRigidbody.GetComponent<Aim>().OnGetArrow();
        }
        if (other.attachedRigidbody.GetComponent<Player>() != null && !_isPlayerShot) {
            other.attachedRigidbody.GetComponent<Player>().OnHit(_enemy);
        }
        if (Controllable.CurrentUnderControl.transform == other.attachedRigidbody.transform && other.attachedRigidbody.GetComponent<BigElf>() != null ) {
            other.attachedRigidbody.GetComponent<BigElf>().OnHit(_enemy);
        }

        Destroy(gameObject);
    }
}