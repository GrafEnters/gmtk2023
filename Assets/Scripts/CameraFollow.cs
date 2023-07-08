using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public bool isCustomOffset;
    public Vector3 offset;

    public float smoothSpeed = 0.1f;

    private void Start()
    {
        // You can also specify your own offset from inspector
        // by making isCustomOffset bool to true
        target = Controllable.CurrentUnderControl.transform;
        if (!isCustomOffset && target)
        {
            offset = transform.position - target.position;
        }
    }

    private void LateUpdate() {
        target = Controllable.CurrentUnderControl.transform;
        SmoothFollow();   
    }

    public void SmoothFollow()
    {
        Vector3 targetPos = target.position + offset;
        Vector3 smoothFollow = Vector3.Lerp(transform.position,
            targetPos, smoothSpeed);

        transform.position = smoothFollow;
        transform.LookAt(target);
    }
}