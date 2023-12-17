using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefInfo : MonoBehaviour
{
    public static System.DateTime GetLastSpinTime()
    {
        if (!PlayerPrefs.HasKey("LastSpinTime"))
        {
            SetLastSpinTime(System.DateTime.Now.AddDays(-1));
        }
        return System.DateTime.Parse(PlayerPrefs.GetString("LastSpinTime"));
    }
    public static void SetLastSpinTime(System.DateTime time)
    {
        PlayerPrefs.SetString("LastSpinTime", time.ToString());
    }
    public static void SetTotalExtraLife(int total)
    {
        PlayerPrefs.SetInt("TotalExtraLife", total);
    }
    public static int TotalExtraLife()
    {
        return PlayerPrefs.GetInt("TotalExtraLife", 5);
    }

    public static float Distance(Vector3 a, Vector3 b)
    {
        return Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
    }

    public static System.DateTime GetLastDieTime()
    {
        return System.DateTime.Parse(PlayerPrefs.GetString("LastDieTime", System.DateTime.Now.ToString()));
    }
    public static void SetLastDieTime(System.DateTime time)
    {
        PlayerPrefs.SetString("LastDieTime", time.ToString());
    }

    public static int GetPlayTime(string id = "")
    {
        return PlayerPrefs.GetInt("PlayTime" + id, 0);
    }
    public static void SetPlayTime(int c, string id = "")
    {
        PlayerPrefs.SetInt("PlayTime" + id, c);
    }
    public static void IncreasePlayTime(int am, string id = "")
    {
        PlayerPrefs.SetInt("PlayTime" + id, GetPlayTime(id) + am);
    }
    //
    public static float GetIQ()
    {
        return PlayerPrefs.GetFloat("IQ", 85);
    }
    public static void SetIQ(float c)
    {
        PlayerPrefs.SetFloat("IQ", c);
    }

    public static int GetHeart()
    {
        if (PrefInfo.IsUnlimited()) return TotalExtraLife();
        return Mathf.Max(0, PlayerPrefs.GetInt("Heart", TotalExtraLife()));
    }
    public static void SetHeart(int c)
    {
        PlayerPrefs.SetInt("Heart", c);
    }
    //public static int GetSpin()
    //{
    //    return PlayerPrefs.GetInt("Spin", 0);
    //}
    //public static void SetSpin(int c)
    //{
    //    PlayerPrefs.SetInt("Spin", c);
    //}
    //public static int GetGem()
    //{
    //    return PlayerPrefs.GetInt("Gem", 0);
    //}
    //public static void SetGem(int c)
    //{
    //    PlayerPrefs.SetInt("Gem", c);
    //}
    //public static void AddGem(int c)
    //{
    //    PlayerPrefs.SetInt("Gem", GetGem() + c);
    //}

    public static bool GetItemStatus(int id)
    {
        if (id == 0) return true;
        return PlayerPrefs.GetInt("Equipment_" + id, 0) != 0;
    }
    public static void SetItemStatus(int id, bool status = true)
    {
        PlayerPrefs.SetInt("Equipment_" + id, status ? 1 : 0);
    }


    public static string GetName()
    {
        return PlayerPrefs.GetString("PlayerName", "You");
    }
    public static void SetName(string name)
    {
        PlayerPrefs.SetString("PlayerName", name);
    }
    public static bool IsUnlimited()
    {
        return PlayerPrefs.GetInt("UnlimitedHeart", 0) == 1;
    }
    public static void SetUnlimited(bool active = false)
    {
        PlayerPrefs.SetInt("UnlimitedHeart", active ? 1 : 0);
    }
    public static bool IsUsingAd()
    {
        return PlayerPrefs.GetInt("AdEnabled", 1) == 1;
    }
    public static void SetAd(bool active = false)
    {
        PlayerPrefs.SetInt("AdEnabled", active ? 1 : 0);
    }
    public static float GetSensitivity()
    {
        return PlayerPrefs.GetFloat("Sensitivity", 0.5f);
    }
    public static void SetSensitivity(float total)
    {
        PlayerPrefs.SetFloat("Sensitivity", total);
    }
    public static int GetCurrentSkin(int type)
    {

        return PlayerPrefs.GetInt("Skin" + type, 0);
    }
    public static void SetCurrentSkin(int c, int type)
    {
        PlayerPrefs.SetInt("Skin" + type, c);
    }




    public static int GetTotalStar(GameMode mode)
    {
        return PlayerPrefs.GetInt("TotalStar_" + mode, 0);
    }
    public static void SetTotalStar(int totalStar, GameMode mode)
    {
        PlayerPrefs.SetInt("TotalStar_" + mode, totalStar);
    }

    //public static int GetCoin()
    //{
    //    return PlayerPrefs.GetInt("Coin", 0);
    //}

    //public static void SetCoin(int total)
    //{
    //    PlayerPrefs.SetInt("Coin", total);
    //}
    //public static void AddCoin(int total)
    //{
    //    PlayerPrefs.SetInt("Coin", GetCoin() + total);
    //}

    public static string GetLevelStats(int level, GameMode mode)
    {
        return PlayerPrefs.GetString("Stats_" + level + "_" + mode, "0" + (level == 1 ? "1" : "0"));
    }
    public static void SetLevelStats(int level, GameMode mode, int totalStar, bool isUnlocked)
    {
        PlayerPrefs.SetString("Stats_" + level + "_" + mode, string.Format("{0}{1}", totalStar, isUnlocked ? 1 : 0));
    }

    public static int GetLevelStar(int level, GameMode mode)
    {
        return PlayerPrefs.GetInt("Star" + level + "_" + mode, 0);
    }

    public static void SetLevelStar(int level, GameMode mode, int star)
    {
        if (star > PlayerPrefs.GetInt("Star" + level + "_" + mode, 0))
        {
            PlayerPrefs.SetInt("Star" + level + "_" + mode, star);
        }
    }

    public static bool IsUnlocked(int id, int type)
    {
        if ((id == 0 && type == 0)) return true;
        return PlayerPrefs.GetInt("Lock_" + type + "_" + id, 0) == 0 ? false : true;
    }

    public static void SetUnlocked(int id, bool active, int type)
    {
        PlayerPrefs.SetInt("Lock_" + type + "_" + id, active ? 1 : 0);
        //FireBaseManager.Instance.LogEvent("CHARACTER_UNLOCK_" + (id + 1));
    }

    //public static void SetSpin(int currentSpin)
    //{
    //    PlayerPrefs.SetInt("Spin", currentSpin);
    //}

    //public static int GetSpin()
    //{
    //    return PlayerPrefs.GetInt("Spin", 1);
    //}


    private static string CurrentDailyIndexStr = "CurrentDailyIndex";
    public static void SetCurrentDailyGiftIndex()
    {
        PlayerPrefs.SetInt(CurrentDailyIndexStr, CurrentDailyIndex() + 1);
        if (CurrentDailyIndex() > 6)
        {
            if (Complete7Day() == false)
            {
                SetComplete7Day();

            }
            PlayerPrefs.SetInt(CurrentDailyIndexStr, 0);
        }
        SetDayClaimed();
    }

    private static string DayClaimed = "DayClaimed";
    public static int GetDayClaim()
    {
        return PlayerPrefs.GetInt(DayClaimed, 0);
    }
    public static void SetDayClaimed()
    {
        PlayerPrefs.SetInt(DayClaimed, GetDayClaim() + 1);
    }

    public static int CurrentDailyIndex()
    {
        return PlayerPrefs.GetInt(CurrentDailyIndexStr);
    }

    private static string Complete7DayStr = "Complete7Day";
    public static bool Complete7Day()
    {
        return PlayerPrefs.GetInt(Complete7DayStr, 0) == 0 ? false : true;
    }

    public static void SetComplete7Day()
    {
        Debug.Log("Complete 7 day");
        PlayerPrefs.SetInt(Complete7DayStr, 1);
    }

    private static string LastDayLoginStr = "LastDayLogin";
    public static void SaveTimeLastLogin(string time)
    {
        PlayerPrefs.SetString(LastDayLoginStr, time);
    }

    public static string TimeLastDayLogin()
    {
        return PlayerPrefs.GetString(LastDayLoginStr);
    }


    #region WaterSort
    private static bool hasFirstDayLogin => PlayerPrefs.HasKey("FIRSTDAYJOINEDGAME");
    public static void SetFirstDayLogin() => PlayerPrefs.SetString("FIRSTDAYJOINEDGAME", System.DateTime.Now.Date.ToString());
    public static System.DateTime GetFirstDayLogin()
    {
        if (hasFirstDayLogin)
            return System.DateTime.Parse(PlayerPrefs.GetString("FIRSTDAYJOINEDGAME"));
        else
        {
            SetFirstDayLogin();
            return System.DateTime.Now.Date;
        }
    }

    public static int GetCurrentTubeSkin() => PlayerPrefs.GetInt("CurrentTubeSkin", 0);
    public static void SetCurrentTubeSkin(int skin) => PlayerPrefs.SetInt("CurrentTubeSkin", skin);

    public static int GetCurrentBG() => PlayerPrefs.GetInt("CurrentBG", 0);
    public static void SetCurrentBG(int bg) => PlayerPrefs.SetInt("CurrentBG", bg);


    public static bool IsUnlockedSkin(int type, int skinID)
    {
        if (skinID == 0) return true;
        return PlayerPrefs.GetInt("UnlockedSkinWS_" + type + "_" + skinID, 0) == 1;
    }

    public static void SetUnlockedSkin(int type, int skinID)
    {
        PlayerPrefs.SetInt("UnlockedSkinWS_" + type + "_" + skinID, 1);
    }

    #region Calendar Daily Challenge
    /// <summary>
    /// 0 = no medal, 1 = brozen, 2 = silver, 3 = gold
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static int GetMedalAtDate(System.DateTime date) => PlayerPrefs.GetInt("MedalAtDate_" + date.Date.ToString(), 0);
    public static void SetMedalAtDate(System.DateTime date, int medal) => PlayerPrefs.SetInt("MedalAtDate_" + date.Date.ToString(), medal);

    public static int GetDailyChallengeStreak() => PlayerPrefs.GetInt("DailyChallengeStreak", 0);
    public static void SetDailyChallengeStreak(int streak) => PlayerPrefs.SetInt("DailyChallengeStreak", streak);

    public static int GetMonthStreak(int year, int month) => PlayerPrefs.GetInt("HighestStreakMonth_" + month + "Year_" + year, 0);
    public static void SetMontStreak(int year, int month, int streak) => PlayerPrefs.SetInt("HighestStreakMonth_" + month + "Year_" + year, streak);
    public static bool HasMonthTrophy(int year, int month) => PlayerPrefs.GetInt("TrophyMonth_" + month + "Year_" + year, 0) == 1;
    public static void SetOwnedMonthTrophy(int year, int month) => PlayerPrefs.SetInt("TrophyMonth_" + month + "Year_" + year, 1);


    public static bool HasLastDayLogin() => PlayerPrefs.HasKey("LastDayLoginWaterSort");
    public static System.DateTime GetLastDayLogin() => System.DateTime.Parse(PlayerPrefs.GetString("LastDayLoginWaterSort", "01/01/1990"));
    public static void SetLastDayLogin(System.DateTime date) => PlayerPrefs.SetString("LastDayLoginWaterSort", date.Date.ToString());

    public static bool IsClaimChallengeGift(int index) => PlayerPrefs.GetInt("DailyChallengeGiftClaimed" + index, 0) == 1;
    public static void SetIsClaimChallengeGift(int index, bool isClaimed = true) => PlayerPrefs.GetInt("DailyChallengeGiftClaimed" + index, isClaimed ? 1 : 0);
    #endregion

    #region DailyGift
    public static int GetStreakDailyGift() => PlayerPrefs.GetInt("DailyGiftStreak", 1);
    public static void SetStreakDailyGift(int streak) => PlayerPrefs.SetInt("DailyGiftStreak", streak);
    public static bool IsClaimedDailyGift(int streak) => PlayerPrefs.GetInt("DailyGiftClaimed" + streak, 0) == 1;
    public static void SetIsClaimedDailyGift(int streak, bool isClaimed) => PlayerPrefs.SetInt("DailyGiftClaimed" + streak, isClaimed ? 1 : 0);
    #endregion

    #region Challenge
    public static bool IsFinishLevel(int level, GameMode mode)
    {
        return PlayerPrefs.GetInt("FinishModeChallenge_" + mode + "_" + level, 0) == 1;
    }

    public static void SetIsFinishLevel(int level, GameMode mode)
    {
        PlayerPrefs.SetInt("FinishModeChallenge_" + mode + "_" + level, 1);
    }

    public static bool IsPayLevel(int level, GameMode mode)
    {
        return PlayerPrefs.GetInt("PayLevel_" + mode + "_" + level, 0) == 1;
    }    
    
    public static void SetIsPayLevel(int level, GameMode mode)
    {
        PlayerPrefs.SetInt("PayLevel_" + mode + "_" + level, 1);
    }

    public static int GetTotalUnlockedLevel(GameMode mode)
    {
        return PlayerPrefs.GetInt("TotalUnlockedLevel_" + mode, 1);
    }

    public static void SetTotalUnlockedLevel(int total, GameMode mode)
    {
        PlayerPrefs.SetInt("TotalUnlockedLevel_" + mode, total);
    }

    public static bool IsUnlockHardLevel() => PlayerPrefs.GetInt("UnlockHardChallenge", 0) == 1;
    public static void SetIsUnlockHardedLevel() => PlayerPrefs.SetInt("UnlockHardChallenge", 1);
    #endregion

    #region LuckySpin
    public static int GetSpinTime() => PlayerPrefs.GetInt("LuckySpinTime", 0);
    public static void SetSpinTime(int spin) => PlayerPrefs.SetInt("LuckySpinTime", spin);
    #endregion


    #region progress
    public static int GetCurrentProgLevel() => PlayerPrefs.GetInt("ProgressLevel", 1);
    public static void SetCurrentProgLevel(int level) => PlayerPrefs.SetInt("ProgressLevel", level);


    public static int GetCurrentIDProgressTube() => PlayerPrefs.GetInt("CurrentProgIndexTube", 1);
    public static int GetCurrentIDProgressBG() => PlayerPrefs.GetInt("CurrentProgIndexBG", 1);
    public static int GetCurrentIDProgressChest() => PlayerPrefs.GetInt("CurrentProgIndexChest", 0);    
    
    public static void SetCurrentIDProgressTube(int index) => PlayerPrefs.SetInt("CurrentProgIndexTube", index);
    public static void SetCurrentIDProgressBG(int index) => PlayerPrefs.SetInt("CurrentProgIndexBG", index);
    public static void SetCurrentIDProgressChest(int index) => PlayerPrefs.SetInt("CurrentProgIndexChest", index);
    #endregion


    #region Noti
    public static bool HasNotiShopItem => PlayerPrefs.GetInt("NotiShopItems", 0) == 1;
    public static void SetHasNotiShopItem(bool state = true) => PlayerPrefs.SetInt("NotiShopItems", state ? 1 : 0);

    public static bool HasNotiDailyChallenge => PlayerPrefs.GetInt("NotiDailyChallenge", 0) == 1;
    public static void SetHasNotiDailyChallenge(bool state = true) => PlayerPrefs.SetInt("NotiDailyChallenge", state ? 1 : 0);
    #endregion

    #region mode Unlock
    public static bool isUnlockModeDaily => PlayerPrefs.GetInt("DailyChallengeModeUnlocked", 0) == 1;
    public static void SetisUnlockModeDaily() => PlayerPrefs.SetInt("DailyChallengeModeUnlocked", 1);    
    public static bool isUnlockModeChallenge => PlayerPrefs.GetInt("ChallengeModeUnlocked", 0) == 1;
    public static void SetisUnlockModeChallenge() => PlayerPrefs.SetInt("ChallengeModeUnlocked", 1);
    #endregion
    #endregion

    #region Ad
    public static bool isUsingNativeAds => /*true;//*/ PlayerPrefs.GetInt("Remote_NativeAd", 1) == 1;
    public static int GetInterWatched => PlayerPrefs.GetInt("InterADWatchedFromPlayer", 0);
    public static void AddInterWatched() => PlayerPrefs.SetInt("InterADWatchedFromPlayer", GetInterWatched +1);    
    public static int GetRewardWatched => PlayerPrefs.GetInt("RewardADWatchedFromPlayer", 0);
    public static void AddRewardWatched() => PlayerPrefs.SetInt("RewardADWatchedFromPlayer", GetRewardWatched + 1);

    public static bool OnlyInGameAD => PlayerPrefs.GetInt("AD_INGAME", 1) == 1;

    public static bool IsNoel => /*false; //*/ PlayerPrefs.GetInt("Event_Theme", 0) == 1;
    public static int Popup_AfterOpen => PlayerPrefs.GetInt("Popup_AfterOpen", 0);
    public static string Noel_progress => PlayerPrefs.GetString("Noel_progress", "3,10,24,32");
    #endregion
}
