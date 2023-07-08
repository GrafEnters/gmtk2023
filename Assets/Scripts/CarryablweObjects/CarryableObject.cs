using System.Collections;
using UnityEngine;

namespace DefaultNamespace {
    public class CarryableObject : MonoBehaviour {
        [SerializeField]
        private Collider _collider;

        public Type type;
        public float stunDuration = 1f;

        public enum Type {
            None,
            Box,
            Antenna,
            RemoteController
        }
        
        public enum State {
            IsCarrying,
            IsDropped,
            IsStunning
        }
        public void SetState(State state) {
            _collider.enabled = state == State.IsDropped;
        }

        public void OnDrop() {
            SetState(CarryableObject.State.IsStunning);
            transform.SetParent(null);
            StartCoroutine(WaitForStunEnd());
        }

        private IEnumerator WaitForStunEnd() {
            yield return new WaitForSeconds(stunDuration);
            SetState(CarryableObject.State.IsDropped);
        }
    }
}