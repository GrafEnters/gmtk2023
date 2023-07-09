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
            _spine.SetAnimation("walk");
            float degrees = angle * Mathf.Rad2Deg % 360;
            if (degrees >= 315 && degrees <= 360 || degrees >= 0 && degrees <= 135) {
                RotateSpriteHorizontallyWhenMove(Vector3.left);
            } else {
                RotateSpriteHorizontallyWhenMove(Vector3.right);
            }
        } else {
            base.MoveAnimation();
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

        yield return new WaitForSeconds(1.5f);

        _navMeshAgent.isStopped = false;
        _isWaitingArriving = true;
        _navMeshAgent.avoidancePriority = 0;
        _navMeshAgent.speed = defaultSpeed * 5;
        _navMeshAgent.SetDestination(CurrentUnderControl.transform.position);
        // Vector3 vector = CurrentUnderControl.transform.position - transform.position;
        // RaycastHit[] hits = Physics.RaycastAll(transform.position, vector.normalized, 20f);
        // RaycastHit hit = hits.FirstOrDefault(hit => hit.collider.tag == "Obstacle");
        // if (hit.collider != null) {
        //     _navMeshAgent.speed = defaultSpeed;
        //     _navMeshAgent.SetDestination(hit.point);
        // }
    }
    
    private void OnTriggerEnter(Collider other) {
        if (_isAttacking) {
            if (other.tag == "Player") {
                _navMeshAgent.destination = transform.position;
                _navMeshAgent.isStopped = true;
                _navMeshAgent.speed = defaultSpeed;
                
                CurrentUnderControl.OnHit(this);

                _isAttacking = false;
                _isWaitingArriving = false;
                _navMeshAgent.avoidancePriority = 50;
                Stun();
            }
        } 
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        if (_isAttacking && _isWaitingArriving) {
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance) {
                _navMeshAgent.destination = transform.position;
                _navMeshAgent.isStopped = true;

                _isAttacking = false;
                _isWaitingArriving = false;

                _navMeshAgent.speed = defaultSpeed;
                _navMeshAgent.avoidancePriority = 50;
                Stun();
            }
        }
    }
}