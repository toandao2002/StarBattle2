using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_ANDROID
using Unity.Notifications.Android;
using Sirenix.OdinInspector;
#endif

#if UNITY_IOS
using Unity.Notifications.iOS;
#endif
using Random = UnityEngine.Random;

public class LocalNotificationManager : MonoBehaviour
{
    public static LocalNotificationManager Instance;
    [ReadOnly]
    public bool isAd = false;
    [SerializeField]
    private string[] notiTitleTexts;
    [SerializeField]
    private string[] notiTexts;
    [SerializeField]
    private string[] notiDailyTexts;
    [SerializeField]
    private string[] notiEnergyTexts;
    // Use this for initialization
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            CreateChannel();
        }
    }
    public void Init()
    {
    }
    public void OnApplicationPause(bool pause)
    {
        if (isAd) return;
        if (pause)
        {
            SetNotification();
            //SetNotiDailyReward();
            //try
            //{
            //    if (HeartManager.Instance.UseHeartCounter == HeartManager.StateDropdown.On)
            //    {
            //        SetNotiEnergy(System.TimeSpan.FromMinutes((HeartManager.MAX_HEART - HeartManager.Instance.currentHeart) * HeartManager.REFILL_TIME));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.Log("ec ec");
            //}

        }
    }
    public void Set(int id, TimeSpan delay, string title, string message, int type)
    {

        System.DateTime localTime = System.DateTime.Now.AddMinutes(delay.TotalMinutes);
        double delayLocal = 0;



        if (localTime.Hour >= 0 && localTime.Hour <= 9)
        {
            delayLocal = (10 - localTime.Hour) * 60;
        }

        delay = delay.Add(TimeSpan.FromMinutes(delayLocal));

#if UNITY_IOS

        DateTime temp = System.DateTime.Now.Add(delay);

        var calendarTrigger = new iOSNotificationCalendarTrigger()
        {
            Year = temp.Year,
            Month = temp.Month,
            Day = temp.Day,
            Hour = temp.Hour,
            Minute = temp.Minute,
            Second = temp.Second,
            Repeats = false
        };

        var notification = new iOSNotification()
        {
            // You can optionally specify a custom identifier which can later be 
            // used to cancel the notification, if you don't set one, a unique 
            // string will be generated automatically.
            Identifier = id.ToString(),
            Title = title,
            Body = message,
            Subtitle =  Application.productName,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = calendarTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);
#endif

#if UNITY_ANDROID
        Debug.Log("SET NOTIFIACTION :" + delay.ToString() + " " + title + " \n" + message + "\n" + DateTime.Now.Add(delay));
        DateTime temp = System.DateTime.Now.Add(delay);

        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = message;
        notification.FireTime = temp;

        AndroidNotificationCenter.SendNotificationWithExplicitID(notification, Application.identifier, id);

#endif
    }


    string notiRewardText;
    /// <summary>
    /// push noti at 9am everyday
    /// </summary>
    public void SetNotiDailyReward()
    {
        string title = "Boom!";
        DateTime now = DateTime.Now;
        try
        {
            Cancel(10000);

        }
        catch { Debug.Log("CANCEL ERROR 4104124"); }

        string version = SystemInfo.operatingSystem;
        try
        {
            if (version.Contains("4.0") || version.Contains("4.1") || version.Contains("4.2") || version.Contains("4.3") || version.Contains("4.4") ||
                version.Contains("5.0") || version.Contains("5.1") || version.Contains("5.2"))
            {

            }
            else
            {
                string[] iconTexts = new string[]{
                    "🎁",
                    "🤩",
                    "⏰",
                    "🎉"
                };
                notiRewardText = iconTexts[Random.Range(0, iconTexts.Length)] + notiDailyTexts[Random.Range(0, notiDailyTexts.Length)] + iconTexts[Random.Range(0, iconTexts.Length)];
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }

#if UNITY_IOS
        var calendarTrigger = new iOSNotificationCalendarTrigger()
        {
            Year = now.Year,
            Month = now.Month,
            Day = now.Day,
            Hour = 9,
            Repeats = false
        };

        var notification = new iOSNotification()
        {
            // You can optionally specify a custom identifier which can later be 
            // used to cancel the notification, if you don't set one, a unique 
            // string will be generated automatically.
            Identifier = 10000.ToString(),
            Title = title,
            Body = message,
            Subtitle =  Application.productName,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = calendarTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);
#endif
#if UNITY_ANDROID
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = notiRewardText;
        notification.FireTime = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0).AddDays(1);
        AndroidNotificationCenter.SendNotificationWithExplicitID(notification, Application.identifier, 10000);
#endif

    }


    string notiEnergyText;
    /// <summary>
    /// push noti when full energy
    /// </summary>
    public void SetNotiEnergy(TimeSpan delay)
    {
        string title = "Energy 100% Recharge!";
        System.DateTime localTime = System.DateTime.Now.AddMinutes(delay.TotalMinutes);
        double delayLocal = 0;



        if (localTime.Hour >= 0 && localTime.Hour <= 9)
        {
            delayLocal = (10 - localTime.Hour) * 60;
        }

        delay = delay.Add(TimeSpan.FromMinutes(delayLocal));
        try
        {
            Cancel(11000);
        }
        catch { Debug.Log("CANCEL ERROR 4104124"); }

        string version = SystemInfo.operatingSystem;
        try
        {
            if (version.Contains("4.0") || version.Contains("4.1") || version.Contains("4.2") || version.Contains("4.3") || version.Contains("4.4") ||
                version.Contains("5.0") || version.Contains("5.1") || version.Contains("5.2"))
            {

            }
            else
            {
                string[] iconTexts = new string[]{
                    "🤩",
                    "⚡",
                    "💪",
                    "❤️"
                };
                notiEnergyText = iconTexts[Random.Range(0, iconTexts.Length)] + notiEnergyTexts[Random.Range(0, notiEnergyTexts.Length)] + iconTexts[Random.Range(0, iconTexts.Length)];
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        Debug.Log("SET ENERGY NOTI :" + delay.ToString() + " " + title + "\n" + DateTime.Now.Add(delay));
#if UNITY_IOS
  DateTime temp = System.DateTime.Now.Add(delay);

        var calendarTrigger = new iOSNotificationCalendarTrigger()
        {
            Year = temp.Year,
            Month = temp.Month,
            Day = temp.Day,
            Hour = temp.Hour,
            Minute = temp.Minute,
            Second = temp.Second,
            Repeats = false
        };

        var notification = new iOSNotification()
        {
            // You can optionally specify a custom identifier which can later be 
            // used to cancel the notification, if you don't set one, a unique 
            // string will be generated automatically.
            Identifier = 10000.ToString(),
            Title = title,
            Body = message,
            Subtitle =  Application.productName,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = calendarTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);
#endif
#if UNITY_ANDROID
        DateTime temp = System.DateTime.Now.Add(delay);
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = notiEnergyText;
        notification.FireTime = temp;
        AndroidNotificationCenter.SendNotificationWithExplicitID(notification, Application.identifier, 11000);
#endif

    }


    public void CancelAll()
    {
        for (int i = 0; i < 20; i++)
        {
            Cancel(i);
            Cancel(100 + i);
        }
    }
    public void Cancel(int id)
    {

#if UNITY_ANDROID
        AndroidNotificationCenter.CancelAllScheduledNotifications();
#elif UNITY_IOS
        for (int i = 0; i < iOSNotificationCenter.GetScheduledNotifications().Length; i++)
        {
            iOSNotification noti = iOSNotificationCenter.GetScheduledNotifications()[i];
            if (noti.Identifier.ToString().Equals(id + ""))
            {
                Debug.Log("CANCEL: " + id + " " + noti.Identifier);
                iOSNotificationCenter.RemoveScheduledNotification(noti.Identifier);
                break;
            }
        }
        iOSNotification noti = iOSNotificationCenter.GetScheduledNotifications()[10000];
                    if (noti.Identifier.ToString().Equals(id + ""))
            {
                Debug.Log("CANCEL: " + id + " " + noti.Identifier);
                iOSNotificationCenter.RemoveScheduledNotification(noti.Identifier);
            }
        noti = iOSNotificationCenter.GetScheduledNotifications()[11000];
                    if (noti.Identifier.ToString().Equals(id + ""))
            {
                Debug.Log("CANCEL: " + id + " " + noti.Identifier);
                iOSNotificationCenter.RemoveScheduledNotification(noti.Identifier);
            }
#endif
    }

    public void LogNotificationParams()
    {
        Debug.Log("CHECK CLICK NOTI");
#if UNITY_ANDROID
#elif UNITY_IOS

#endif
    }

    public async void SetNotification()
    {
        await Cysharp.Threading.Tasks.UniTask.WhenAny(Cysharp.Threading.Tasks.UniTask.WaitUntil(() => GameManger.instance!=null), Cysharp.Threading.Tasks.UniTask.Delay(5000, ignoreTimeScale:  true));
        double[] remindPlayTimes = { 2 * 60, 24 * 60, 24 * 60 * 3, 24 * 60 * 5, 24 * 60 * 7, 24 * 60 * 14, 24 * 60 * 21, 24 * 60 * 28 };
        Debug.Log("SET NOTIFICATION ");
        try
        {
            for (int i = 0; i < notiTexts.Length; i++)
            {
                Cancel(i);
            }
        }
        catch { Debug.Log("CANCEL ERROR 4104124"); }
        string[] text1 = { "Let's play!", "Let's go!!" };

        string[] text2 = new string[notiTexts.Length];
        for (int i = 1; i <= notiTexts.Length; i++)
        {
            text2[i - 1] = notiTexts[i - 1];
        }
        string version = SystemInfo.operatingSystem;
        try
        {
            if (version.Contains("4.0") || version.Contains("4.1") || version.Contains("4.2") || version.Contains("4.3") || version.Contains("4.4") ||
                version.Contains("5.0") || version.Contains("5.1") || version.Contains("5.2"))
            {

            }
            else
            {
                string[] iconTexts = new string[]{
                    "🎁",
                    "🤩",
                    "⏰",
                    "🎉",
                    "🥳",
                    "💪"
                };

                for (int i = 1; i <= notiTexts.Length; i++)
                {
                    text2[i - 1] = iconTexts[Random.Range(0, iconTexts.Length)] + notiTexts[i - 1] + iconTexts[Random.Range(0, iconTexts.Length)];
                }

            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }



        for (int i = 0; i < notiTexts.Length; i++)
        {
            int type = i;
            int index = Random.Range(0, notiTexts.Length);
            try
            {
                Set(
                    i,
                    System.TimeSpan.FromMinutes(remindPlayTimes[i]),
                    notiTitleTexts[index], text2[index],
                    type);
            }
            catch
            {
                index = Random.Range(0, notiTexts.Length);
                Set(
                   i,
                   System.TimeSpan.FromMinutes(remindPlayTimes[i]),
                    notiTitleTexts[index], text2[index],
                   type);
            }
        }
    }
    //
    bool isInitChannel = false;
    public void CreateChannel()
    {
        if (isInitChannel) return;
        isInitChannel = true;
#if UNITY_ANDROID
        var channel = new AndroidNotificationChannel()
        {
            Id = Application.identifier,
            Name = Application.productName,
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }
#endif

}


