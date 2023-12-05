using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using DG.Tweening;
using UnityEngine.UI;

public class SettingUI : BasePopUP
{
    public static SettingUI instance;

    public BoxSetting sound;
    public BoxSetting music;
    public BoxSetting vibration;
    public BoxSetting darkmode;
    public BoxSetting restorePurchased;
    public BoxSetting howToPlay;
     
    public List<Color> bgrColors;
    private void OnEnable()
    {
        MyEvent.ChangeTheme += ChangeTheme;
    }
    private void OnDisable()
    {
        MyEvent.ChangeTheme -= ChangeTheme;
    }
    public override void Show(object data = null, int dir = 1)
    {
        main.SetActive(true);
        if (rec == null)
        {
            rec = main.GetComponent<RectTransform>();
        }
        float posY = Screen.width;
        if (dir == -1) // down
        {
            posY = 120;
        }
        rec.DOAnchorPos3DX(0, durationEffect).From(posY * dir).SetEase(ease);
        UpdateUi();
        ChangeTheme();
    }
    public override void Hide(int dir = 1)
    {
        if (rec == null)
        {
            rec = main.GetComponent<RectTransform>();
        }

        float posY = -Screen.width;
        if (dir == 1) // up
        {
            posY = -120;
        }

        rec.DOAnchorPos3DX(posY * dir, durationEffect).SetEase(ease).From(0).OnComplete(() => {
            main.SetActive(false);
        });

    }
    public void Back()
    {
        Hide(-1);
    }
    public void UpdateUi()
    {
        SettingData settingData = SettingData.GetSetting();
        if(settingData.musicVolume > 0)
        {
            music.mySwitch.UpdateState(true);
        }
        else
        {

            music.mySwitch.UpdateState(false);
        }
        if (settingData.soundVolume > 0)
        {
            sound.mySwitch.UpdateState(true);
        }
        else
        {

            sound.mySwitch.UpdateState(false);
        }
        
        music.mySwitch.UpdateState(settingData.vibrate);
        if (settingData.theme == NameTheme.Dark)
        {
            darkmode.mySwitch.UpdateState(true);
        }
        else
        {

            darkmode.mySwitch.UpdateState(false);
        }
         
        
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this; 
        }
        
    }

    public void TurnSound()
    {
        ManageAudio.Instacne.UpdateSound();
    }
    public void TurnMusic()
    {
        ManageAudio.Instacne.UpdateMusic();
    }
    public void TurnHaptic()
    {
        ManageAudio.Instacne.UpdateHaptic();
    }
    public void ChangeDarkMode()
    {
        SettingData settingData = SettingData.GetSetting();
        
        NameTheme nameTheme =  settingData.ChangeTheme();
        GameConfig.instance.nameTheme = nameTheme;
         
        MyEvent.ChangeTheme?.Invoke();
    }

    public override void ChangeTheme()
    {
        base.ChangeTheme();
        NameTheme theme = GameConfig.instance.nameTheme;
        if (theme == NameTheme.Dark)
        {
            BgrMain.color= bgrColors[1];
        }
        else
        {
            BgrMain.color = bgrColors[0];

        }

    }

    
}
