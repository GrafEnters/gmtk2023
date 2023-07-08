using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinableRock : MonoBehaviour
{
        public void OnMined() {
                Destroy(gameObject);
        }
}
