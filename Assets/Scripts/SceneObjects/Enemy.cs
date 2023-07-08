using System.Collections;
using UnityEngine;

public abstract class Enemy : Controllable {
    private const float SetDestinationRepeat = 1f;
    public float ReachTargetDistance = 1;

    protected float stunDuration = 5f;
    protected bool _isStunned;

    protected override void FixedUpdate() {
        base.FixedUpdate();
        TrySetDestination();
    }

    public void Stun() {
    public virtual void Stun() {
        StartCoroutine(WaitForStunEnd());
    }

    private void TrySetDestination() {
        if (_isUnderControl) {
            return;
        }

        _navMeshAgent.destination = CurrentUnderControl.transform.position;
        if (_navMeshAgent.remainingDistance <= ReachTargetDistance) {
            ReachTarget(CurrentUnderControl);
        }
    }

    public void Knockback(Vector3 dir) {
        _navMeshAgent.Move(dir);
    }

    protected virtual void OnStunEnd() { }

    private IEnumerator WaitForStunEnd() {
        _navMeshAgent.isStopped = true;
        _rb.detectCollisions = false;
        _rb.velocity = Vector3.zero;
        _isStunned = true;
        yield return new WaitForSeconds(stunDuration);
        _isStunned = false;
        _navMeshAgent.isStopped = false;
        _rb.detectCollisions = true;
        OnStunEnd();
    }

    public override void StartControl() {
        base.StartControl();
        _navMeshAgent.isStopped = true;
    }

    public override void EndControl() {
        base.EndControl();
        _navMeshAgent.isStopped = false;
    }

    protected abstract void ReachTarget(Controllable target);
}