using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class InputHeroName : MonoBehaviour {
    [SerializeField]
    private TMP_InputField _input;

    [SerializeField]
    private Animation _animation;

    private bool _isHeroNameSaved = false;
    
    

    public IEnumerator WaitForHeroName(Action callback) {
        yield return new WaitWhile(() => !_isHeroNameSaved);
        yield return StartCoroutine(WaitForHideAnim());
        callback?.Invoke();
    }

    public void OnApply() {
        if (string.IsNullOrEmpty(_input.text)) {
            _animation.Play("Wrong");
            return;
        }

        SaveDataManager.Data.HeroName = _input.text;
        EntryPoint.SaveDataManager.Save();
        _isHeroNameSaved = true;
    }

    private IEnumerator WaitForHideAnim() {
        _animation.Play("Hide");
        yield return new WaitWhile(() => _animation.isPlaying);
    }
}