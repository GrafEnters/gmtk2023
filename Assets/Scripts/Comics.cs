using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Comics : MonoBehaviour {
    [SerializeField]
    private List<Animation> _animations;

    private const string anim_name = "Comics_";
    private bool _isWaitingForTap;

    public void Init() {
        StartCoroutine(ShowingCoroutine());
    }

    private IEnumerator ShowingCoroutine() {
        for (int i = 0; i < _animations.Count; i++) {
            yield return StartCoroutine(Enumerator(i));
        }
        SceneManager.LoadScene(1);
    }

    private IEnumerator Enumerator(int stepIndex) {
        _animations[stepIndex].Play(anim_name + (stepIndex + 1));
        yield return new WaitWhile(() => _animations[stepIndex].isPlaying);

        _isWaitingForTap = true;
        yield return new WaitWhile(() => _isWaitingForTap);
    }

    public void OnComicsClick() {
        _isWaitingForTap = false;
    }
}