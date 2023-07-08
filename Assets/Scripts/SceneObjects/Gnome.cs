using System.Collections;
using UnityEngine;

public class Gnome : Enemy {
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    
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
        Debug.Log("Main gnome ability casted!");
    }

    public override void EndControl() {
        base.EndControl();
        Stun();
    }

    protected override void ReachTarget(Controllable target) {
    }
}