using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class Fairy : Enemy {
    protected bool _isTeleporting;

    [SerializeField]
    private float _maxDistanceToTp = 1;

    protected override void MainAbility() {
        if (_isTeleporting) {
            return;
        }

        IsLockedMovement = true;
        _isTeleporting = true;
        StartCoroutine(TeleportCoroutine(true));
    }

    protected IEnumerator TeleportCoroutine(bool isPlayer) {
        GameObject go = FindNearObjectWithTag("Crystal");

        if (isPlayer) {
            go = FindNearObjectWithTag("Crystal");
        } else {
            go = CurrentUnderControl.gameObject;
        }

        if (go == null) {
            _isTeleporting = false;
            _isAttacking = false;
            IsLockedMovement = false;
            yield break;
        }

        yield return ShowTeleportAnimation();
        if ((transform.position - go.transform.position).magnitude >= _maxDistanceToTp) {
            _isTeleporting = false;
            _isAttacking = false;
            IsLockedMovement = false;
            yield break;
        }

        Teleport(go);
        if (isPlayer) {
            if (this is BigFairy) {
                go.GetComponent<Crystal>().OnTeleportedByBigPlayer();
            } else {
                go.GetComponent<Crystal>().OnTeleportedByPlayer();
            }
        }

        yield return new WaitForSeconds(0.1f);
        _isTeleporting = false;
        _isAttacking = false;
        IsLockedMovement = false;
    }

    protected IEnumerator ShowTeleportAnimation() {
        if (!_spine) {
            yield break;
        }

        yield return StartCoroutine(_spine.ShowSpineAnimation("attack"));
        _spine.SetAnimation("idle");
    }

    private void Teleport(GameObject obj) {
        Transform target = TeleportHelper.Instance.GetRandomTarget;
        obj.transform.position = target.position;
        if (obj.CompareTag("Crystal")) {
            obj.transform.SetParent(target);
        }
    }

    public override void EndControl() {
        base.EndControl();
        _isTeleporting = false;
        _isAttacking = false;
    }

    protected override void ReachTarget(Controllable target) {
        if (_isAttacking) {
            return;
        }

        IsLockedMovement = true;
        _isAttacking = true;
        StartCoroutine(TeleportCoroutine(false));
    }
}