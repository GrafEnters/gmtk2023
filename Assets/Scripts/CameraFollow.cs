using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public float smoothSpeed = 0.1f;

    public bool isCustomOffset;
    public Vector3 offset;

    private Transform _target;

    private void Start() {
        FindTarget();
        if (!isCustomOffset) {
            offset = transform.position - _target.position;
        }
    }

    private void LateUpdate() {
        SmoothFollow();
    }

    private void FindTarget() {
        _target = Controllable.CurrentUnderControl.transform;
    }

    private void SmoothFollow() {
        Vector3 targetPos = _target.position + offset;
        Vector3 smoothFollow = Vector3.Slerp(transform.position,
            targetPos, smoothSpeed);

        transform.position = smoothFollow;
    }
}