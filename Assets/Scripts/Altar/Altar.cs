using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace {
    public class Altar : MonoBehaviour {

        public Action OnAltarAssembled;
        [SerializeField]
        private List<AltarSpot> _spots = new List<AltarSpot>();
        private List<CarryableObject.Type> _collected = new List<CarryableObject.Type>();

        private void Start() {
            foreach (AltarSpot altarSpot in _spots) {
                altarSpot.SetCollectCallback(OnCollected);
            }
        }

        private void OnCollected(CarryableObject.Type collectedType) {
            _collected.Add(collectedType);
            if (_collected.Count == _spots.Count) {
                OnAltarAssembled?.Invoke();
            }
        }
    }
}