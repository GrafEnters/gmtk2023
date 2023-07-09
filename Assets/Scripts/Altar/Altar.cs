using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZhukovskyGamesPlugin;

namespace DefaultNamespace {
    public class Altar : MonoBehaviour {

        public float ActivateDuration;
        public Action OnAltarAssembled;

        [SerializeField]
        private CarryableObject _first;
        [SerializeField]
        private CarryableObject _second;
        [SerializeField]
        private CarryableObject _third;
        [SerializeField]
        private List<AltarSpot> _spots = new List<AltarSpot>();
        private static List<CarryableObject.Type> _collected = new List<CarryableObject.Type>();
        private long _startActivatingTime;

        private bool _isActivating;
        private Player _activator;
        private Vector3 _startedActivatorPosition;

        private void Start() {
            Dictionary<CarryableObject.Type, CarryableObject> map =
                new Dictionary<CarryableObject.Type, CarryableObject>() {
                    { CarryableObject.Type.Box, _first },
                    { CarryableObject.Type.Antenna, _second },
                    { CarryableObject.Type.RemoteController, _third },
                };
            
            foreach (AltarSpot altarSpot in _spots) {
                altarSpot.SetCollectCallback(OnCollected);
                if (_collected.Contains(altarSpot.type)) {
                    altarSpot.PutCarryableObjecToSpot(Instantiate(map[altarSpot.type]));
                }
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
                UIManager.Instance.ShowBottomText("PRESS 'E' TO INTERACT WITH PORTAL", Color.white);
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
            SceneManager.LoadScene("FinalScene");
            EntryPoint.Audio.PlaySound(Sounds.portal_open);
        }

        public void InterruptActivating() {
            _isActivating = false;
            _startActivatingTime = 0;
            _spots.ForEach(spot => spot.InterruptRiceAnimation());
        }
    }
}