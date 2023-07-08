using System.Collections;
using UnityEngine;

public abstract class Enemy : Controllable {

    protected float stunDuration = 5f;
    protected bool _isStunned;

    public virtual void Stun() {
        StartCoroutine(WaitForStunEnd());
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
}