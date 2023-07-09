using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class MainAntagonist : Enemy {
    public Animation Animation;
    public SpeechPopUp PopUp;

    private bool _isSpeaking;
    private bool _isWaitingArriving;
    private bool _isDefending = true;
    
    public float defaultSpeed = 3.5f;
    public float defenderRadius = 2f;
    public float defenderSpeed = 0.01f;
    private float angle;

    protected override bool IsSupportReincarnation => false;
    
    public void SetSpeakMode(bool isSpeaking) {
        _isSpeaking = isSpeaking;
    }

    public void SetDefendingMode(bool isDefending) {
        _isDefending = isDefending;
    }
    
    protected override void TrySetDestination() {
        if (_isAttacking || _isSpeaking) {
            return;
        }

        if (_isUnderControl || _isStunned) {
            _navMeshAgent.isStopped = true;
            return;
        }

        float distance = Vector3.Distance(CurrentUnderControl.transform.position, transform.position);
        if (distance > BlindZoneOut && _isDefending) {
            angle += defenderSpeed;
            float x = Vector3.zero.x + defenderRadius * Mathf.Cos(angle);
            float z = Vector3.zero.z + defenderRadius * Mathf.Sin(angle);
            Vector3 newPosition = new Vector3(x, transform.position.y, z);
            _isDefending = true;
            MoveAnimation();
            transform.position = newPosition;
        } else {
            BlindZoneOut = 50;
            _isDefending = false;
            base.TrySetDestination();
        }
    }

    protected override void MoveAnimation() {
        if (_isDefending) {
            float degrees = angle * Mathf.Rad2Deg % 360;
            if (degrees >= 315 && degrees <= 360 || degrees >= 0 && degrees <= 135) {
                RotateSpriteHorizontallyWhenMove(Vector3.left);
            } else {
                RotateSpriteHorizontallyWhenMove(Vector3.right);
            }
            _spine.SetAnimation("walk", true);
        } else {
            if (_isAttacking) {
                RotateSpriteHorizontallyWhenMove(_navMeshAgent.velocity);
            } else {
                base.MoveAnimation();
            }
        }
    }

    protected override IEnumerator WaitForStunEnd() {
        _navMeshAgent.isStopped = true;
        _rb.detectCollisions = false;
        _rb.velocity = Vector3.zero;
        _isStunned = true;
        
        yield return StartCoroutine(_spine.ShowSpineAnimation("hurt"));
        _spine.SetAnimation("idle");

        _isStunned = false;
        _navMeshAgent.isStopped = false;
        _rb.detectCollisions = true;
        OnStunEnd();
    }

    protected override void MainAbility() {
        Debug.Log("Main gnome ability casted!");
    }
    
    public override void EndControl() {
        base.EndControl();
        Stun();
    }

    protected override void ReachTarget(Controllable target) {
        _navMeshAgent.isStopped = true;
        StartCoroutine(PrepareToAttack());
    }

    private IEnumerator PrepareToAttack() {
        _isAttacking = true;

        yield return _spine.ShowSpineAnimation("charging");

        float distance = Vector3.Distance(CurrentUnderControl.transform.position, transform.position);
        if (distance > ReachTargetDistance + 1f) {
            _isAttacking = false;
            _navMeshAgent.isStopped = false;
            _isWaitingArriving = false;
            _navMeshAgent.SetDestination(CurrentUnderControl.transform.position);
            yield break;
        }
        
        _navMeshAgent.isStopped = false;
        _isWaitingArriving = true;
        _navMeshAgent.avoidancePriority = 0;
        _navMeshAgent.speed = 100f;
        _navMeshAgent.SetDestination(CurrentUnderControl.transform.position);
        
        yield return _spine.ShowSpineAnimation("rush");
        
        distance = Vector3.Distance(CurrentUnderControl.transform.position, transform.position);

        if (distance <= 2f) {
            StartCoroutine(Test());
        }
        
        yield return _spine.ShowSpineAnimation("attack");
       
        _navMeshAgent.destination = transform.position;
        _navMeshAgent.isStopped = true;

        _isAttacking = false;
        _isWaitingArriving = false;

        _navMeshAgent.speed = defaultSpeed;
        _navMeshAgent.avoidancePriority = 50;
    }

    private IEnumerator Test() {
        yield return new WaitForSeconds(0.2f);
        CurrentUnderControl.OnHit(this);
    }
    
    private void OnTriggerEnter(Collider other) {
         if (!_isAttacking) {
            if (other.tag == "Player") {
                CurrentUnderControl.OnHit(this);
            }
        } 
    }
}