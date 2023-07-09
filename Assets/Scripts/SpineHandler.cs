using System.Collections;
using System.Linq;
using Spine.Unity;
using UnityEngine;

namespace DefaultNamespace {
    public class SpineHandler : MonoBehaviour {

        [SerializeField]
        private SkeletonAnimation _spine;

        public void SetAnimation(string key, bool loop = false) {
            if (!_spine) {
                return;
            }
            if (_spine.AnimationState.GetCurrent(0)?.Animation.Name != key) {
                _spine.AnimationState.SetAnimation(0, key, loop);
            }
        }
        
        public void SetSpineWalkOrIdle(Vector3 dir) {
            if (!_spine) {
                return;
            }

            if (dir.magnitude == 0) {
                if (_spine.AnimationState.GetCurrent(0).Animation.Name != "idle") {
                    _spine.AnimationState.SetAnimation(0, "idle", true);
                }
            } else {
                if (_spine.AnimationState.GetCurrent(0).Animation.Name != "walk") {
                    _spine.AnimationState.SetAnimation(0, "walk", true);
                }
            }
        }

        public IEnumerator ShowSpineAnimation(string key) {
            SetAnimation(key);
            float dur = _spine.skeletonDataAsset.GetSkeletonData(true).Animations.FirstOrDefault(p => p.Name == key).Duration;

            yield return new WaitForSeconds(dur);
        }
    }
}