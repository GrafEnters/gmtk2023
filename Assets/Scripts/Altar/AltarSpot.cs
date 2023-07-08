using System;
using UnityEngine;

namespace DefaultNamespace {
    public class AltarSpot : MonoBehaviour {
        public CarryableObject.Type type;
        [SerializeField]
        private Collider Collider;
        private Action<CarryableObject.Type> _onCollected1;

        public void SetCollectCallback(Action<CarryableObject.Type> onCollected) {
            _onCollected1 = onCollected;
        }

        private void OnTriggerEnter(Collider other) {
            Player player = other.GetComponent<Player>();
            if (player) {
                if (player.TryGetCarryableObjectByType(type, out CarryableObject targetObject)) {
                    targetObject.SetState(CarryableObject.State.OnAltar);
                    targetObject.transform.SetParent(transform.parent.transform);
                    targetObject.transform.localPosition = transform.localPosition;
                    Collider.enabled = false;
                    _onCollected1?.Invoke(type);
                }
            }
        }
    }
}