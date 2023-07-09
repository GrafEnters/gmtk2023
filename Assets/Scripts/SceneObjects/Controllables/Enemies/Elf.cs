using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elf : Enemy {
    [SerializeField]
    protected Arrow _arrowPrefab;

    protected bool _isReloading;

    [SerializeField]
    protected float _arrowSpeed = 0.5f;
    [SerializeField]
    protected float _reloadTime = 2.5f;

    protected override void MainAbility() {
        Vector3 dir = GetRaycastDirection();
        TryShootTarget(transform.position + dir, true);
    }

    private void TryShootTarget(Vector3 target, bool isPlayer) {
        if (_isReloading) {
            return;
        }

        _isReloading = true;
        IsLockedMovement = true;
        StartCoroutine(ShotTarget(target, isPlayer));
    }

    public override void StartControl() {
        base.StartControl();
        _isReloading = false;
    }

    protected virtual IEnumerator ShotTarget(Vector3 target, bool isPlayer) {
        if (!_spine) {
            _isReloading = false;
            IsLockedMovement = false;
            yield break;
        }

        yield return StartCoroutine(_spine.ShowSpineAnimation("attack"));
        ShootAbility(target, isPlayer);
        _spine.SetAnimation("idle");
        if (!_isUnderControl) {
            yield return new WaitForSeconds(_reloadTime);
        }
     
        _isReloading = false;
        IsLockedMovement = false;
    }

    protected virtual void ShootAbility(Vector3 target, bool isPlayer) {
        Arrow arrow = Instantiate(_arrowPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        RotateSpriteHorizontallyWhenMove(target - transform.position);
        if (isPlayer) {
            arrow.SetFlyDirection(target - transform.position, _arrowSpeed, isPlayer);
        } else {
            arrow.SetFlyDirection(target - transform.position, _arrowSpeed, isPlayer, this); 
        }
    }

    protected override void ReachTarget(Controllable target) {
        TryShootTarget(target.transform.position, false);
    }
}