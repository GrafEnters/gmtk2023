using UnityEngine;

public class MainAntagonist : Enemy {
    private const float SetDestinationRepeat = 1f;
    protected override bool IsSupportReincarnation => false;

    private void Start() {
        InvokeRepeating(nameof(TrySetDestination), 0f, SetDestinationRepeat);
    }

    private void TrySetDestination() {
        if (_isUnderControl) {
            return;
        }

        _navMeshAgent.destination = CurrentUnderControl.transform.position;
        if (_navMeshAgent.remainingDistance > 50) {
            Debug.LogError("Too much dist. remaining");
        }
    }

    protected override void MainAbility() {
        Debug.Log("Main gnome ability casted!");
    }

    public override void EndControl() {
        base.EndControl();
        Stun();
    }
}