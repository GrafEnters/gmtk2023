using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhukovskyGamesPlugin;

[CreateAssetMenu(fileName = "_AudioSo", menuName = "ScriptableObjects/AudioSo", order = 0)]
public class AudioSo : ScriptableObject {
    
    [SerializeField]
    public SerializableDictionary<Music, AudioClip> _musicClips;
    [SerializeField]
    public SerializableDictionary<Sounds , AudioClip > _soundClips;
    
}