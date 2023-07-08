using UnityEngine;

public class MainAntagonist : Enemy {
    protected override void MainAbility() {
        Debug.Log("Main gnome ability casted!");
    }

    public override void EndControl() {
        base.EndControl();
        Stun();
    }

    protected override void ReachTarget(Controllable target) {
        target.OnHit(this);
    }
}