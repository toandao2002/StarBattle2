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
public enum nameVibrate
{
    light,
    medium,
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
    Queue<nameVibrate> stateVibrates = new Queue<nameVibrate>();
    private void Awake()
    {

        Instacne = this;
    }
     
    IEnumerator DelaySound()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); 
            if(stateVibrates.Count != 0)  
                stateVibrates.Dequeue(); 
        }
    }
    
    public void Start()
    {
        settingData = SettingData.GetSetting();
        UpdateSettingSoundAndMusic();
        StartCoroutine(DelaySound());
    }
    public SettingData GetSetting()
    {
        return settingData;
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
    public void UpdateAutoDot()
    {
        settingData.TurnAutoDot();
        
    }
    public void UpdateHaptic()
    {
        settingData.ChangeVibration();
        VibrateLight();
    }

    public void VibrateLight()
    {
        haptic.LightFeedback();
    }
    public void VibratMedium()
    {
        if (settingData.vibrate)
        {
            if (!stateVibrates.Contains(nameVibrate.light))
            {
                stateVibrates.Enqueue(nameVibrate.medium);
                haptic.MediumFeedback(); 
            }
        }
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
