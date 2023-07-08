using System.Collections;
using UnityEngine;

public class Gnome : Enemy {
    private const float SetDestinationRepeat = 1f;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private void Start() {
        InvokeRepeating(nameof(TrySetDestination), 0f, SetDestinationRepeat);
    }

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