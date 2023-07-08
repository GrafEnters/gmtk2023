using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Spine.Unity;
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

   

    private List<CarryableObject> _carryableObjects = new List<CarryableObject>();
    private bool _isAttacking;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            StartControl();
        }
    }

    protected override void MainAbility() {
        if (_isAttacking) {
            return;
        }

        _isAttacking = true;
        IsLockedMovement = true;
        StartCoroutine(AbilityAnimation());
    }

    private IEnumerator AbilityAnimation() {
        _spine.AnimationName = "attack";
        _spine.state.SetAnimation(0, "attack", false);
        float dur = _spine.skeletonDataAsset.GetSkeletonData(true).Animations.FirstOrDefault(p => p.Name == "attack")
            .Duration;
        _spine.state.AddAnimation(0, "idle", false, 0);
        yield return new WaitForSeconds(dur);
        AbilityEffect();
        _isAttacking = false;
        IsLockedMovement = false;
    }

    private void AbilityEffect() {
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

    protected override void OnStepOverCarryableObject(CarryableObject carryableObject) {
        base.OnStepOverCarryableObject(carryableObject);
        carryableObject.transform.SetParent(transform);
        Vector3 position = new Vector3 {
            z = -0.6f,
            y = 0,
            x = Random.Range(-1f, 1f)
        };
        carryableObject.transform.localPosition = position;

        carryableObject.SetState(CarryableObject.State.IsCarrying);
        _carryableObjects.Add(carryableObject);
    }

    public override void OnHit(Controllable from) {
        if (!gameObject.activeSelf) {
            return;
        }

        base.OnHit(from);

        if (_carryableObjects.Count > 0) {
            _carryableObjects.ForEach(o => o.OnDrop());
            _carryableObjects.Clear();
        }

        float multiplier = 1;
        if (from is Gnome) {
            multiplier = 0.1f;
        }

        if (from is BigGnome) {
            multiplier = 0.2f;
        }

        Vector3 offset = transform.position - from.transform.position;
        _navMeshAgent.Move(offset.normalized * (1.05f * multiplier));
    }
    
    public bool TryGetCarryableObjectByType(CarryableObject.Type type, out CarryableObject carryableObject) {
        carryableObject = _carryableObjects.FirstOrDefault(o => o.type == type);
        if (carryableObject) {
            _carryableObjects.Remove(carryableObject);
        }
        return carryableObject != null;
    }
}