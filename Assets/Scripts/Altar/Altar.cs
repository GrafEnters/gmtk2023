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

        private void Start() {
            foreach (AltarSpot altarSpot in _spots) {
                altarSpot.SetCollectCallback(OnCollected);
            }
        }

        private void OnCollected(CarryableObject.Type collectedType) {
            _collected.Add(collectedType);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.GetComponent<Player>()) {
                UIManager.Instance.ShowBottomText("PRESS E TO OPEN PORTAL", Color.green);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.GetComponent<Player>()) {
                UIManager.Instance.HideBottomText();
            }          
        }

        public bool StartActivating() {
            if (_collected.Count < _spots.Count || _isActivating) {
                return false;
            }
            
            long leftMilliseconds = (DateTime.Now.Ticks - _startActivatingTime ) / TimeSpan.TicksPerMillisecond;
            if (leftMilliseconds < ActivateDuration * 1000f) {
                return false;
            }

            _startActivatingTime = DateTime.Now.Ticks;
            _isActivating = true;
            
            _spots.ForEach(spot => spot.ShowRiceAnimation());
            return true;
        }

        public void InterruptActivating() {
            _isActivating = false;
            _startActivatingTime = 0;
            _spots.ForEach(spot => spot.InterruptRiceAnimation());
        }
    }
}