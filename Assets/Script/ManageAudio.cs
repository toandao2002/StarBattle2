using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NameSound
{

}
public enum NameMusic
{

}
public class ManageAudio : MonoBehaviour
{
    public static ManageAudio Instacne;
    public AudioSource msc;
    public AudioSource sound;
    public List<AudioClip> sounds;
    public List<AudioClip> musics;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public void PlaySound(NameSound id, bool loop = false)
    {
        sound.PlayOneShot(sounds[(int)id]);
    }
    public void UpdateSettingSoundAndMusic()
    {
        SettingData settingData = SettingData.GetSetting();

        msc.volume = settingData.musicVolume;
        sound.volume = settingData.soundVolume;
    }
}
