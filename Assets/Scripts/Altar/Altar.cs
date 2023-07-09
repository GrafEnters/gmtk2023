using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace {
    public class Altar : MonoBehaviour {

        public float ActivateDuration;
        public Action OnAltarAssembled;
        [SerializeField]
        private List<AltarSpot> _spots = new List<AltarSpot>();
        private List<CarryableObject.Type> _collected = new List<CarryableObject.Type>();
        private long _startActivatingTime;

        private bool _isActivating;
        private Player _activator;
        private Vector3 _startedActivatorPosition;

        private void Start() {
            foreach (AltarSpot altarSpot in _spots) {
                altarSpot.SetCollectCallback(OnCollected);
            }
        }

        private void Update() {
            if (_isActivating && _activator) {
                if (_startedActivatorPosition != _activator.transform.position) {
                    InterruptActivating();
                }
            }
        }

        private void OnCollected(CarryableObject.Type collectedType) {
            _collected.Add(collectedType);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.GetComponent<Player>()) {
                UIManager.Instance.ShowBottomText("PRESS 'E' TO OPEN PORTAL", Color.white);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.GetComponent<Player>()) {
                UIManager.Instance.HideBottomText();
            }          
        }

        public bool StartActivating(Player activator) {
            if (_collected.Count < _spots.Count || _isActivating) {
                return false;
            }
            
            long leftMilliseconds = (DateTime.Now.Ticks - _startActivatingTime ) / TimeSpan.TicksPerMillisecond;
            if (leftMilliseconds < ActivateDuration * 1000f) {
                return false;
            }

            _activator = activator;
            _startedActivatorPosition = _activator.transform.position;

            _startActivatingTime = DateTime.Now.Ticks;
            _isActivating = true;

            for (int i = 0; i < _spots.Count; i++) {
                _spots[i].ShowRiceAnimation(null);
                if (i == _spots.Count - 1) {
                    _spots[i].ShowRiceAnimation(OnActivated);
                }
            }
            return true;
        }

        private void OnActivated() {
            
        }

        public void InterruptActivating() {
            _isActivating = false;
            _startActivatingTime = 0;
            _spots.ForEach(spot => spot.InterruptRiceAnimation());
        }
    }
}