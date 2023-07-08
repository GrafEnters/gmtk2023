using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Gnome : Enemy {
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private bool _isAttacking;

    public override void Stun() {
        base.Stun();
        ChangeSpriteAlpha(0.5f);
    }

    protected override void OnStunEnd() {
        base.OnStunEnd();
        ChangeSpriteAlpha(1f);
    }

    private void ChangeSpriteAlpha(float percent) {
        if(_spriteRenderer) {
            _spriteRenderer.color =
                new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, percent);
        }
    }

    protected override void MainAbility() {
        TryBreakObjectWithTag("Rock");
    }

    public override void EndControl() {
        base.EndControl();
        Stun();
    }

    protected override void ReachTarget(Controllable target) {
        if (_isAttacking) {
            return;
        }

        IsLockedMovement = true;
        _isAttacking = true;
        StartCoroutine(AttackCoroutine(target));
    }

    private IEnumerator AttackCoroutine(Controllable target) {
        _navMeshAgent.isStopped = true;
        if (!_spine) {
            yield break;
        }
        _spine.state.SetAnimation(0, "attack", false);
        float dur = _spine.skeletonDataAsset.GetSkeletonData(true).Animations.FirstOrDefault(p => p.Name == "attack")
            .Duration;
        _spine.state.AddAnimation(0, "idle", false, 0);
        yield return new WaitForSeconds(dur);
        if (IsCloseToTarget) {
            target.OnHit(this);
        }

        yield return new WaitForSeconds(0.5f);
        _isAttacking = false;
        IsLockedMovement = false;
        _navMeshAgent.isStopped = false;
    }
}