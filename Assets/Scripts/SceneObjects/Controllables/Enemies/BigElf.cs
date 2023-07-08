using UnityEngine;

public class BigElf : Elf {
    protected override void ShootAbility(Vector3 target, bool isPlayer) {
        Arrow arrow = Instantiate(_arrowPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);

        Vector3 dir = (target - transform.position).normalized;
        if (isPlayer) {
            arrow.SetFlyDirection(Quaternion.Euler(0, 15, 0) * dir, _arrowSpeed, isPlayer);
        } else {
            arrow.SetFlyDirection(Quaternion.Euler(0, 15, 0) * dir, _arrowSpeed, isPlayer, this);
        }

        arrow = Instantiate(_arrowPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        if (isPlayer) {
            arrow.SetFlyDirection(Quaternion.Euler(0, -15, 0) * dir, _arrowSpeed, isPlayer);
        } else {
            arrow.SetFlyDirection(Quaternion.Euler(0, -15, 0) * dir, _arrowSpeed, isPlayer, this);
        }
    }
}