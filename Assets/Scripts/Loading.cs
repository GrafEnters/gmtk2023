using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace {
    public class Loading : MonoBehaviour {
        [SerializeField]
        private float fakeLoadingInSeconds;

        [SerializeField]
        private Slider _loadingSlider;

        public void SetProgress(float percent) {
            _loadingSlider.SetValueWithoutNotify(percent);
        }

        public void FakeProgress() { }


        public void StartFakeLoading(Action callback) {
            StartCoroutine(ProgressCoroutine(callback));
        }
        public IEnumerator ProgressCoroutine(Action callback) {
            float time = 0;
            while (time < fakeLoadingInSeconds) {
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
                float percent = time / fakeLoadingInSeconds;

                SetProgress(Mathf.Min(percent, 1));
            }
            callback?.Invoke();
        }
    }
}