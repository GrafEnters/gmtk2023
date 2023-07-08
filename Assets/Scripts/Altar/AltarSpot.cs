using System;
using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace {
    public class AltarSpot : MonoBehaviour {
        public CarryableObject.Type type;
        [SerializeField]
        private Collider Collider;
        public float duration = 5f;

        private Tween _currentTween;
        private CarryableObject _currentCarryableObject;
        private Action<CarryableObject.Type> _onCollected;

        public void PutCarryableObjecToSpot(CarryableObject carryableObject) {
            _currentCarryableObject = carryableObject;
            _currentCarryableObject.SetState(CarryableObject.State.OnAltar);
            _currentCarryableObject.transform.SetParent(transform.parent.transform);
            _currentCarryableObject.transform.localPosition = transform.localPosition;
            Collider.enabled = false;
            _onCollected?.Invoke(_currentCarryableObject.type);
        }
        
        public void ShowRiceAnimation() {
            if (_currentCarryableObject == null) {
                return;
            }

            Vector3 altarCenter = transform.parent.position;
            altarCenter.y = 3; 
            _currentTween = _currentCarryableObject.transform.DOMove(altarCenter, duration);
        }

        public void InterruptRiceAnimation() {
            if (_currentCarryableObject == null) {
                return;
            }
            _currentTween?.Kill();
            _currentCarryableObject.transform.DOMove(transform.position, 1f);
        }

        public void SetCollectCallback(Action<CarryableObject.Type> onCollected) {
            _onCollected = onCollected;
        }
    }
}