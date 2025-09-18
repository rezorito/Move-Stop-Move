using UnityEngine;

[CreateAssetMenu(fileName = "soundData", menuName = "Data/Sound Data")]
public class SoundData : ScriptableObject {
    public SoundName soundName;
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume = 1f;
    public bool loop;

    public enum SoundName {
        Music_MainNormal,
        Music_MainZombie,
        Music_BgrPlay,
        Attack, 
        Button_Click,
        Lose,
        Win,
        LevelUp,
        TimeCountDown,
        WeaponHit,
    }
}