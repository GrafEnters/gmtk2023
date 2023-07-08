using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : Controllable {
    [SerializeField]
    protected NavMeshAgent _navMeshAgent;

    protected float stunDuration = 5f;
    protected bool _isStunned;

    public void Stun() {
        StartCoroutine(WaitForStunEnd());
    }

    private IEnumerator WaitForStunEnd() {
        _navMeshAgent.isStopped = true;
        _rb.detectCollisions = false;
        _rb.useGravity = false;
        _rb.velocity = Vector3.zero;
        _isStunned = true;
        yield return new WaitForSeconds(stunDuration);
        _isStunned = false;
        _navMeshAgent.isStopped = false;
        _rb.detectCollisions = true;
        _rb.useGravity = true;
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