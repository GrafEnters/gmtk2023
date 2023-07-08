using UnityEngine;

public class Player : Controllable {
    public static Player Instance;
    public bool IsUnderControl => _isUnderControl;

    [Header("BlasAbility")]
    [SerializeField]
    private float length = 2.5f;

    [SerializeField]
    private float width = 1.5f;

    [SerializeField]
    private float knockbackPower = 0.3f;

    private void Awake() {
        Instance = this;
        StartControl();
    }

    protected override void MainAbility() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 150, LayerMask.GetMask("Floor"))) {
            Vector3 dir = (hit.point - transform.position);
            dir.y = 0;
            dir = dir.normalized;
            RaycastHit[] enemies = Physics.SphereCastAll(new Ray(transform.position + dir * width / 2, dir), width,
                length,
                LayerMask.GetMask("Enemy"));
            foreach (var enemyHit in enemies) {
                Vector3 hitDir = (enemyHit.point - transform.position).normalized;
                enemyHit.rigidbody.GetComponent<Enemy>().Knockback(hitDir * knockbackPower);
                enemyHit.rigidbody.GetComponent<Enemy>().Stun();
            }
        }
    }

    public override void StartControl() {
        base.StartControl();
        gameObject.SetActive(true);
    }

    public override void EndControl() {
        base.EndControl();
        gameObject.SetActive(false);
    }
}