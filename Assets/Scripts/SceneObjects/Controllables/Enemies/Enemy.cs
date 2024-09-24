using System;
using System.Collections;
using UnityEngine;

public abstract class Enemy : Controllable {
    public Action OnStun;
    public Action OnUnderControl;
    public float ReachTargetDistance = 1;
    public float BlindZoneOut = 4f;

    [SerializeField]
    protected float distanceToBreak = 2f;

    protected bool _isStunned;
    protected bool _isAttacking;

    protected override void FixedUpdate() {
        base.FixedUpdate();
        TrySetDestination();
    }

    public virtual void Stun() {
        StartCoroutine(WaitForStunEnd());
        OnStun?.Invoke();
    }

    private void LateUpdate() {
        if (IsLockedMovement) {
            return;
        }

        if (!_isUnderControl) {
            MoveAnimation();
        }
    }

    protected virtual void MoveAnimation() {
        RotateSpriteHorizontallyWhenMove(_navMeshAgent.velocity);
        _spine.SetSpineWalkOrIdle(_navMeshAgent.velocity);
    }

    protected virtual void TrySetDestination() {
        if (_isAttacking) {
            return;
        }

        if (_isUnderControl || _isStunned) {
            _navMeshAgent.isStopped = true;
            return;
        }

        float distance = Vector3.Distance(CurrentUnderControl.transform.position, transform.position);
        if (distance <= BlindZoneOut) {
            _navMeshAgent.destination = CurrentUnderControl.transform.position;
            if (IsCloseToTarget) {
                ReachTarget(CurrentUnderControl);
            }
        }
    }

    protected bool IsCloseToTarget => _navMeshAgent.remainingDistance <= ReachTargetDistance;

    protected GameObject FindNearObjectWithTag(string tag) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 150, LayerMask.GetMask("Default"))) {
            if (!hit.rigidbody) {
                return null;
            }
            if (hit.rigidbody.CompareTag(tag) &&
                (hit.transform.position - transform.position).magnitude < distanceToBreak) {
                return hit.rigidbody.gameObject;
            }
        }

        return null;
    }

    public void Knockback(Vector3 dir) {
        _navMeshAgent.Move(dir);
    }

    protected virtual void OnStunEnd() { }

    protected virtual IEnumerator WaitForStunEnd() {
        _navMeshAgent.isStopped = true;
        _rb.detectCollisions = false;
        _rb.linearVelocity = Vector3.zero;
        _isStunned = true;
        
        yield return StartCoroutine(_spine.ShowSpineAnimation("stunned"));
        _spine.SetAnimation("idle");

        _isStunned = false;
        _navMeshAgent.isStopped = false;
        _rb.detectCollisions = true;
        OnStunEnd();
    }

    public override void StartControl() {
        base.StartControl();
        _isStunned = false;
        _navMeshAgent.isStopped = true;
        OnUnderControl?.Invoke();
    }

    public override void EndControl() {
        base.EndControl();
        _navMeshAgent.isStopped = false;
    }

    protected abstract void ReachTarget(Controllable target);
}