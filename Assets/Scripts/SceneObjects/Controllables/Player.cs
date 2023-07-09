using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class Player : Controllable {
    public static Player Instance;
    public bool IsUnderControl => _isUnderControl;
    public bool IsPressE;
    public bool IsPressSpace;

    public Animation Animation;
    
    [Header("Blast Ability")]
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

    protected override void CheckInputs() {
        base.CheckInputs();
        if (Input.GetKeyDown(KeyCode.E)) {
            IsPressE = true;
        }

        if (Input.GetKeyUp(KeyCode.E)) {
            IsPressE = false;
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
        yield return StartCoroutine(_spine.ShowSpineAnimation("attack"));
        _spine.SetAnimation("idle");
        AbilityEffect();
        _isAttacking = false;
        IsLockedMovement = false;
    }

    private void AbilityEffect() {
        Vector3 dir = GetRaycastDirection();
        if (dir != Vector3.zero) {
            RaycastHit[] enemies = Physics.SphereCastAll(new Ray(transform.position + dir * width / 2, dir), width,
                length,
                LayerMask.GetMask("Enemy"));
            foreach (var enemyHit in enemies) {
                Vector3 hitDir = (enemyHit.point - transform.position).normalized;
                enemyHit.rigidbody.GetComponent<Enemy>().Knockback(hitDir * knockbackPower * 3);
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

        if (IsPressE) {
            IsPressE = false;
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
    }

    private IEnumerator ShowStunnedAnimation() {
        IsLockedMovement = true;
        yield return StartCoroutine(_spine.ShowSpineAnimation("get_hit"));
        IsLockedMovement = false;
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
            multiplier = 0.4f;
        }

        if (from is BigGnome) {
            multiplier = 0.8f;
        }

        if (from is Elf) {
            multiplier = 0.5f;
        }

        if (from is BigElf) {
            multiplier = 0.5f;
        }

        Vector3 dir = transform.position - from.transform.position;
        dir = dir.normalized * (1.05f * multiplier);
        Knockback(dir);
    }

    private void Knockback(Vector3 dir) {
        _navMeshAgent.Move(dir);
        StartCoroutine(ShowStunnedAnimation());
    }

    public bool TryPopCarryableObjectByType(CarryableObject.Type type, out CarryableObject carryableObject) {
        carryableObject = _carryableObjects.FirstOrDefault(o => o.type == type);
        if (carryableObject) {
            _carryableObjects.Remove(carryableObject);
        }

        return carryableObject != null;
    }

    protected override void OnTriggerStay(Collider other) {
        base.OnTriggerStay(other);
        AltarSpot spot = other.GetComponent<AltarSpot>();
        if (spot) {
            if (spot != null && IsPressE) {
                if (TryPopCarryableObjectByType(spot.type, out CarryableObject suitableObject)) {
                    spot.PutCarryableObjecToSpot(suitableObject);
                    IsPressE = false;
                }
            }
        } else {
            Altar altar = other.GetComponent<Altar>();
            if (altar != null && IsPressE) {
                if (altar.StartActivating(this)) {
                    IsPressE = false;
                }
            } else if (altar != null && IsPressSpace) {
                altar.InterruptActivating();
            }
        }
    }
}