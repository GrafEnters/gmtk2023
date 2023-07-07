using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace {
    public class Duck : MonoBehaviour {
        public void OnClick() {
            AddOneQuack();
        }

        private void AddOneQuack() {
            SaveDataManager.Data.Points++;
            EntryPoint.SaveDataManager.Save();
        }

       
    }
}