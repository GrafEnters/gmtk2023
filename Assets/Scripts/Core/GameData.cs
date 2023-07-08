using System.Collections.Generic;
using UnityEngine;
using ZhukovskyGamesPlugin;

namespace DefaultNamespace {
    [System.Serializable]
    public class GameData {
        public int Points;
        
        public int RandomNumber;
        public List<int> numbers;
        public SerializableDictionary<string, int> namesCount;

        public GameData() {
            Points = 0;
            RandomNumber = Random.Range(0, 1001);
            numbers = new List<int>();
        }
    }
}