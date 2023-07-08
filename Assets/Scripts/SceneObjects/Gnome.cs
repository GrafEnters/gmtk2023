using System.Collections;
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
        _spriteRenderer.color =
            new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, percent);
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

        _isAttacking = true;
        StartCoroutine(AttackCoroutine(target));
    }

    private IEnumerator AttackCoroutine(Controllable target) {
        _navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(1);
        if (IsCloseToTarget) {
            target.OnHit(this);
        }

        yield return new WaitForSeconds(0.5f);
        _isAttacking = false;
        _navMeshAgent.isStopped = false;
    }
}