using UnityEngine;
using ZhukovskyGamesPlugin;

namespace DefaultNamespace {
    public class Duck : MonoBehaviour {
        public void OnClick() {
            EntryPoint.Audio.PlaySound(Sounds.Quack);
            AddOneQuack();
        }

        private void AddOneQuack() {
         
            EntryPoint.SaveDataManager.Save();
        }

       
    }
}