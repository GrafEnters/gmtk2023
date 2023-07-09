using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Aim : MonoBehaviour {
    [SerializeField]
    private Animation _animation;

    private bool _isShot;
    private bool _isDestroyOnArrowHit;

    [SerializeField]
    private UnityEvent UnityAction;

    public void OnGetArrow() {
        if (_isShot) {
            return;
        }

        _isShot = true;
        StartCoroutine(WasShotCoroutine());
    }

    private IEnumerator WasShotCoroutine() {
        _animation.Play("WasShot");
        yield return new WaitWhile(() => _animation.isPlaying);


        UnityAction?.Invoke();
        TryDestroy();
    }

    private void TryDestroy() {
        if (_isDestroyOnArrowHit) {
            Destroy(gameObject);
        } else {
            _isShot = false;
            _animation.Play("NewAim");
        }
    }
}