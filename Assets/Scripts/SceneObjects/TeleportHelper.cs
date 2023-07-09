using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace {
    public class TeleportHelper : MonoBehaviour {
        public List<Transform> _tpTargets;

        public static TeleportHelper Instance;

        private void Awake() {
            Instance = this;
            DisableTargetsView();
        }

        private void DisableTargetsView() {
            foreach (var target in _tpTargets) {
                target.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        public Transform GetRandomTarget {
            get {
                List<Transform> tt = _tpTargets.Where(t => t.transform.childCount == 0).ToList();

                return tt[Random.Range(0, tt.Count)].transform;
            }
        }
    }
}