using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class Enemy : Controllable {
    public float ReachTargetDistance = 1;

    protected float stunDuration = 5f;

    [SerializeField]
    protected float distanceToBreak = 2f;

    protected bool _isStunned;

    protected override void FixedUpdate() {
        base.FixedUpdate();
        TrySetDestination();
    }

    public virtual void Stun() {
        StartCoroutine(WaitForStunEnd());
    }

    private void LateUpdate() {
        if (IsLockedMovement) {
            return;
        }

        if (_isUnderControl) { } else {
            RotateSpriteHorizontallyWhenMove(_navMeshAgent.velocity);
            SetSpineWalkOrIdle(_navMeshAgent.velocity);
        }
    }

    private void TrySetDestination() {
        if (_isUnderControl || _isStunned) {
            _navMeshAgent.isStopped = true;
            return;
        }

        _navMeshAgent.destination = CurrentUnderControl.transform.position;
        if (IsCloseToTarget) {
            ReachTarget(CurrentUnderControl);
        }
    }

    protected bool IsCloseToTarget => _navMeshAgent.remainingDistance <= ReachTargetDistance;

    protected GameObject FindNearObjectWithTag(string tag) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 150, LayerMask.GetMask("Default"))) {
            if (hit.rigidbody.CompareTag(tag) &&
                (hit.transform.position - transform.position).magnitude < distanceToBreak) {
                return hit.rigidbody.gameObject;
            }
        }

        return null;
    }

    protected void TryBreakObjectWithTag(string tag) {
        var go = FindNearObjectWithTag(tag);
        if (go != null) {
            go.GetComponent<MinableRock>().OnMined();
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
        _spine.state.SetAnimation(0, "stunned", false);
        float dur = _spine.skeletonDataAsset.GetSkeletonData(true).Animations.FirstOrDefault(p => p.Name == "stunned")
            .Duration;
        _spine.state.AddAnimation(0, "idle", false, 0);


        yield return new WaitForSeconds(dur);
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