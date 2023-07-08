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
    protected float moveSpeed;

    protected bool _isUnderControl;
    private const float MIN_DISTANCE_TO_HAUNT = 1.5f;
    private bool _isMouseIn;
    private bool _isMousePressed;

    protected virtual bool IsSupportReincarnation => true;

    protected virtual void FixedUpdate() {
        if (_isUnderControl) {
            WasdMovement();
        }
    }

    protected void Update() {
        if (_isUnderControl) {
            CheckInputs();
        } else {
            TryGetControl();
        }
    }

    private void TryGetControl() {
        if (_isMouseIn && Input.GetMouseButtonDown(1) && CanBeSwitched) {
            Player.Instance.EndControl();
            StartControl();
        }
    }

    private void CheckInputs() {
        CheckMainAbility();
        SecondAbility();
    }

    private void WasdMovement() {
        Vector3 dir = Vector3.zero;
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        Move(dir * (Time.fixedDeltaTime * moveSpeed));
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

    private void SecondAbility() {
        if (Input.GetMouseButtonDown(1)) {
            if (transform.CompareTag("Player")) {
                return;
            }

            _isMouseIn = false;


            EndControl();
            Player.Instance.StartControl();
            Player.Instance.transform.position = transform.position;
        }
    }

    protected virtual void OnStepOverCarryableObject(CarryableObject carryableObject) {
    }
    
    private void Move(Vector3 dir) {
        _navMeshAgent.Move(dir);
    }

    public virtual void OnHit(Controllable from) {
    }

    public virtual void StartControl() {
        _isUnderControl = true;
        CurrentUnderControl = this;
        SetObvodka(true);
    }

    public virtual void EndControl() {
        _isUnderControl = false;
        SetObvodka(false);
    }

    private bool IsCloseToPlayer => Vector3.Distance(Player.Instance.transform.position, transform.position) <= MIN_DISTANCE_TO_HAUNT;

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

    private void OnTriggerStay(Collider other) {
        CarryableObject targetObject = other.GetComponent<CarryableObject>();
        if (targetObject) {
            OnStepOverCarryableObject(targetObject);
        }
    }
}