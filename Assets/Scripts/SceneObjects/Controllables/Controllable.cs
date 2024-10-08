using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

public abstract class Controllable : MonoBehaviour, IControllable {
    public static Controllable CurrentUnderControl;

    [SerializeField]
    protected NavMeshAgent _navMeshAgent;

    [SerializeField]
    protected GameObject _obvodka;

    [SerializeField]
    protected Rigidbody _rb;

    [SerializeField]
    protected SpineHandler _spine;

    [SerializeField]
    protected Transform _turnContainer;
    
    public SpineHandler Spine => _spine;

    [SerializeField]
    protected float moveSpeed;

    protected bool _isUnderControl;
    private const float MIN_DISTANCE_TO_HAUNT = 1.5f;
    private bool _isMouseIn;
    private bool _isMousePressed;
    public bool IsLockedMovement;

    protected virtual bool IsSupportReincarnation => true;

    protected virtual void Start() {
        if (!IsLockedMovement) {
            _spine.SetAnimation("idle");
        }
    }

    protected virtual void FixedUpdate() {
        if (_isUnderControl && !IsLockedMovement) {
            WasdMovement();
        }
    }

    protected virtual void Update() {
        if (_isUnderControl) {
            CheckInputs();
        } else {
            TryGetControl();
        }
    }

    protected virtual void TryGetControl() {
        if (_isMouseIn && Input.GetMouseButtonDown(1) && CanBeSwitched) {
            Player.Instance.EndControl();
            StartControl();
        }
    }

    protected Vector3 GetRaycastDirection() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 150, LayerMask.GetMask("Floor"))) {
            Vector3 dir = (hit.point - transform.position);
            dir.y = 0;
            return dir.normalized;
        }

        return default;
    }

    protected virtual void CheckInputs() {
        CheckMainAbility();
        TryFreeControllable();
    }

    private void WasdMovement() {
        Vector3 dir = Vector3.zero;
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");

        Move(dir * (Time.fixedDeltaTime * moveSpeed));

        _spine.SetSpineWalkOrIdle(dir);

        RotateSpriteHorizontallyWhenMove(dir);
    }

    protected void RotateSpriteHorizontallyWhenMove(Vector3 dir) {
        if (!_spine) {
            return;
        }

        if (dir.x > 0) {
            Vector3 localScale = _spine.transform.localScale;
            localScale.x = -Mathf.Abs(localScale.x);
            _turnContainer.localScale = localScale;
        }

        if (dir.x < 0) {
            Vector3 localScale = _spine.transform.localScale;
            localScale.x = Mathf.Abs(localScale.x);
            _turnContainer.localScale = localScale;
        }
    }

    private void CheckMainAbility() {
        if (Input.GetMouseButtonDown(0)) {
            MainAbility();
        }
    }

    protected void SetObvodka(bool isActive) {
        if (_obvodka) {
            _obvodka.SetActive(isActive);
        }
    }

    protected abstract void MainAbility();

    protected void TryFreeControllable() {
        if (Input.GetMouseButtonDown(1)) {
            if (transform.CompareTag("Player")) {
                return;
            }

            _isMouseIn = false;


            FreeControllable();
        }
    }

    public void FreeControllable() {
        EndControl();
        Player.Instance.StartControl();
        Player.Instance.transform.position = transform.position;
    }

    protected virtual void OnStepOverCarryableObject(CarryableObject carryableObject) { }

    private void Move(Vector3 dir) {
        _navMeshAgent.Move(dir);
    }

    public virtual void OnHit(Controllable from) { }

    public virtual void StartControl() {
        StopAllCoroutines();
        _isUnderControl = true;
        IsLockedMovement = false;
        CurrentUnderControl = this;
        SetObvodka(true);
    }

    public virtual void EndControl() {
        StopAllCoroutines();
        _isUnderControl = false;
        SetObvodka(false);
    }

    private bool IsCloseToPlayer => Vector3.Distance(Player.Instance.transform.position, transform.position) <=
                                    MIN_DISTANCE_TO_HAUNT;

    private bool CanBeSwitched => !_isUnderControl && !transform.CompareTag("Player") &&
                                  Player.Instance.IsUnderControl && IsCloseToPlayer && IsSupportReincarnation;

    private void OnMouseEnter() {
        _isMouseIn = true;
        if (CanBeSwitched) {
            SetObvodka(true);
        }
    }

    private void OnMouseExit() {
        _isMouseIn = false;
        _isMousePressed = false;
        if (_isUnderControl) {
            return;
        }

        SetObvodka(false);
    }

    protected virtual void OnTriggerStay(Collider other) {
        CarryableObject targetObject = other.GetComponent<CarryableObject>();
        if (targetObject) {
            OnStepOverCarryableObject(targetObject);
        }
    }
}