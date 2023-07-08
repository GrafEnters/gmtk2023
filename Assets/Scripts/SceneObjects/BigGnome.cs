using System.Collections;
using UnityEngine;

public class BigGnome : Gnome {
   
    public override void Stun() { }

    protected override void MainAbility() {
        if (_isBreaking) {
            return;
        }

        _isBreaking = true;
        IsLockedMovement = true;
        StartCoroutine(BigBreakCoroutine());
    }

    private IEnumerator BigBreakCoroutine() {
        GameObject go = FindNearObjectWithTag("BigRock");
        if (go == null) {
            go = FindNearObjectWithTag("Rock");
        }

        if (go == null) {
            _isBreaking = false;
            IsLockedMovement = false;
            yield break;
        }

        yield return ShowAttackAnimation();
        if (go) {
            go.GetComponent<MinableRock>().OnMined();
        }

        yield return new WaitForSeconds(0.2f);
        _isBreaking = false;
        IsLockedMovement = false;
    }

    public override void OnHit(Controllable @from) {
        base.OnHit(@from);
        StopAllCoroutines();
        _isBreaking = false;
        IsLockedMovement = false;
        FreeControllable();
    }
}