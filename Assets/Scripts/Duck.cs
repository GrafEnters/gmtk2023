using UnityEngine;
using ZhukovskyGamesPlugin;

namespace DefaultNamespace {
    public class Duck : MonoBehaviour {
        public void OnClick() {
          
            AddOneQuack();
        }

        private void AddOneQuack() {
         
            EntryPoint.SaveDataManager.Save();
        }

       
    }
}