using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class Controllable : MonoBehaviour, IControllable {
    [SerializeField]
    protected NavMeshAgent _navMeshAgent;

    [SerializeField]
    protected Rigidbody _rb;

    [SerializeField]
    protected float moveSpeed;

    protected bool _isUnderControl;
    private const float MIN_DISTANCE_TO_HAUNT = 1f;

    public static Controllable CurrentUnderControl;

    protected virtual void FixedUpdate() {
        if (_isUnderControl) {
            WasdMovement();
        }
    }

    protected void Update() {
        if (_isUnderControl) {
            CheckInputs();
        }
    }

    private void CheckInputs() {
        CheckMainAbility();
        SecondAbility();
    }

    private void WasdMovement() {
        Vector3 dir = Vector3.zero;
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        Move(dir * (Time.fixedDeltaTime * moveSpeed));
    }

    private void CheckMainAbility() {
        if (Input.GetMouseButtonDown(1)) {
            MainAbility();
        }
    }

    protected abstract void MainAbility();

    private void SecondAbility() {
        if (Input.GetMouseButtonDown(1)) {
            if (transform.CompareTag("Player")) {
                return;
            }

            EndControl();
            Player.Instance.StartControl();
            Player.Instance.transform.position = transform.position;
        }
    }

    private void Move(Vector3 dir) {
        _navMeshAgent.Move(dir);
    }

    public virtual void StartControl() {
        _isUnderControl = true;
        CurrentUnderControl = this;
    }

    public virtual void EndControl() {
        _isUnderControl = false;
    }

    private bool IsCloseToPlayer => Vector3.Distance(Player.Instance.transform.position, transform.position) <=
                                    MIN_DISTANCE_TO_HAUNT;

    private void OnMouseUpAsButton() {
        if (_isUnderControl || transform.CompareTag("Player") || !Player.Instance.IsUnderControl || !IsCloseToPlayer) {
            return;
        }

        Player.Instance.EndControl();
        StartControl();
    }
}