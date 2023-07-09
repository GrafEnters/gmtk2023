using System.Collections;
using UnityEngine;

public class Gnome : Enemy {
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private bool _isAttacking;
    protected bool _isBreaking;

    public override void Stun() {
        base.Stun();
        ChangeSpriteAlpha(0.5f);
    }

    protected override void OnStunEnd() {
        base.OnStunEnd();
        ChangeSpriteAlpha(1f);
    }

    private void ChangeSpriteAlpha(float percent) {
        if (_spriteRenderer) {
            _spriteRenderer.color =
                new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, percent);
        }
    }

    protected override void MainAbility() {
        if (_isBreaking) {
            return;
        }

        IsLockedMovement = true;
        _isBreaking = true;
        StartCoroutine(BreakCoroutine());
    }

    protected IEnumerator BreakCoroutine() {
        GameObject go = FindNearObjectWithTag("Rock");

        if (go == null) {
            _isBreaking = false;
            IsLockedMovement = false;
            yield break;
        }

        yield return ShowAttackAnimation();
        if (go) {
            go.GetComponent<MinableRock>().OnMined();
        }

        yield return new WaitForSeconds(0.1f);
        _isBreaking = false;
        IsLockedMovement = false;
    }

    protected IEnumerator ShowAttackAnimation() {
        if (!_spine) {
            yield break;
        }

        yield return StartCoroutine(_spine.ShowSpineAnimation("attack"));
        _spine.SetAnimation("idle");
    }

    public override void StartControl() {
        base.StartControl();
        _isAttacking = false;
        _isBreaking = false;
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

        yield return StartCoroutine(_spine.ShowSpineAnimation("attack"));
        _spine.SetAnimation("idle");
        if (IsCloseToTarget) {
            target.OnHit(this);
        }

        yield return new WaitForSeconds(0.5f);
        _isAttacking = false;
        IsLockedMovement = false;
        _navMeshAgent.isStopped = false;
    }
}