using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SmartAim : MonoBehaviour {
    [SerializeField]
    private Aim aim1, aim2;

    [SerializeField]
    private float speed, timeBetweenChangeDirection, timeDifference;

    [SerializeField]
    private float timeToHitBoth = 1f;

    [SerializeField]
    private UnityEvent _onBothHit;

    private float multiplier = 1, multiplier2 = -1;
    private int hitAmount = 0;

    private void Start() {
        StartCoroutine(MovingTargets());
    }

    private IEnumerator MovingTargets() {
        float time = 0, time2 = 0;
        for (;;) {
            float calculatedSpeed = Time.deltaTime * speed;
            aim1.transform.position += Vector3.right * calculatedSpeed * multiplier;
            aim2.transform.position += Vector3.left * calculatedSpeed * multiplier2;
            time += Time.deltaTime;
            time2 += Time.deltaTime;
            if (time >= timeBetweenChangeDirection) {
                time = 0;
                multiplier *= -1;
            }

            if (time2 >= timeBetweenChangeDirection + timeDifference) {
                time2 = 0;
                multiplier2 *= -1;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void OnTargetHit() {
        hitAmount++;
        if (hitAmount == 2) {
            _onBothHit?.Invoke();
            StopAllCoroutines();
        }

        StartCoroutine(HitTimeCounter());
    }

    private IEnumerator HitTimeCounter() {
        yield return new WaitForSeconds(timeToHitBoth);
        hitAmount = 0;
    }
}