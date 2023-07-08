using UnityEngine;

public class MainAntagonist : Enemy {
    private void Start() {
        _navMeshAgent.destination = Player.Instance.transform.position;
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        if (_isUnderControl) {
            return;
        }
        if (!Player.Instance.gameObject.activeSelf) {
            _navMeshAgent.destination = _navMeshAgent.transform.position;
        } else if (_navMeshAgent.isOnNavMesh) {
            _navMeshAgent.destination = Player.Instance.transform.position;
        }
    }

    protected override void MainAbility() {
        Debug.Log("Main antagonist ability casted!");
    }
    public override void EndControl() {
        base.EndControl();
        Stun();
    }
}