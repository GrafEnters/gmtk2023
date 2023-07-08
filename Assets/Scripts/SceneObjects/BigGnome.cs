using System.Collections;
using UnityEngine;

public class BigGnome : Gnome {
    private bool _isBreaking;
    public override void Stun() { }

    protected override void MainAbility() {
        if (_isBreaking) {
            return;
        }

        _isBreaking = true;
        StartCoroutine(BreakCoroutine());
    }

    private IEnumerator BreakCoroutine() {
        GameObject go = FindNearObjectWithTag("BigRock");
        if (go == null) {
            go = FindNearObjectWithTag("Rock");
        }

        if (go == null) {
            yield break;
        }

        yield return new WaitForSeconds(1);
        if (go) {
            go.GetComponent<MinableRock>().OnMined();
        }

        yield return new WaitForSeconds(1);
        _isBreaking = false;
    }

    public override void OnHit(Controllable @from) {
        base.OnHit(@from);
        StopAllCoroutines();
        _isBreaking = false;
        FreeControllable();
    }
}