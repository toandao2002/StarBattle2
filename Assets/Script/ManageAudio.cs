using CandyCoded.HapticFeedback;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NameSound
{
    Click,
    ClickCell,
    ErrorPlay,
    Hint,
    Laugh,
    Clap,
    StarClick,
    WinGame,
}
public enum NameMusic
{

}
public class ManageAudio : MonoBehaviour
{
    public static ManageAudio Instacne;
    public AudioSource msc;
    public AudioSource sound;
    public HapticFeedbackController haptic;
    public List<AudioClip> sounds;
    public List<AudioClip> musics;
    SettingData settingData;
    Queue<NameSound> PlayedSound = new Queue<NameSound>();
    private void Awake()
    {

        Instacne = this;
    }
     
    IEnumerator DelaySound()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); 
            if(PlayedSound.Count != 0)  
                PlayedSound.Dequeue(); 
        }
    }
    public void PlaySoundDelay(NameSound id)
    {
        if (!PlayedSound.Contains(id))
        { 
            sound.PlayOneShot(sounds[(int)id]);
            PlayedSound.Enqueue(id);
        }
    }
    public void Start()
    {
        settingData = SettingData.GetSetting();
        UpdateSettingSoundAndMusic();
        StartCoroutine(DelaySound());
    }

    public void UpdateSound( )
    {
        settingData.TurnSound();
        UpdateSettingSoundAndMusic();
    }
    public void UpdateMusic( )
    {
        settingData.TurnMusic();
        UpdateSettingSoundAndMusic();
    }
    public void UpdateHaptic()
    {
        settingData.ChangeVibration();
        Vibrate();
    }

    public void Vibrate()
    {
        if(settingData.vibrate)
            haptic.LightFeedback();
    }
    public void PlaySound(NameSound id )
    {
        sound.PlayOneShot(sounds[(int)id]);
    }
    public void UpdateSettingSoundAndMusic()
    {
        msc.volume = settingData.musicVolume;
        sound.volume = settingData.soundVolume;
    }
}
