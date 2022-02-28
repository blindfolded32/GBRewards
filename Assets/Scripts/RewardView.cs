using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class RewardView : MonoBehaviour
    {

    [Header("Time settings")]
    [SerializeField] public int DayTimeCooldown = 86400;
    [SerializeField] public int WeekTimeCooldown = 604800;
    [SerializeField] public int DayTimeDeadline = 172800;
    [SerializeField] public int WeekTimeDeadline = 345600;
    [Space]

    [Header("RewardSettings")]
    public List<Reward> DailyRewards;
    public List<Reward> WeeklyRewards;

    [Header("UI")]
    [SerializeField] public TMP_Text DailyRewardTimer;
    [SerializeField] public TMP_Text WeeklyRewardTimer;
    [SerializeField] public Transform DailySlotsParent;
    [SerializeField] public Transform WeeklySlotsParent;
    [SerializeField] public SlotRewardView SlotPrefab;
    [SerializeField] public Button ResetButton;
    [SerializeField] public Button GetRewardButton;
    [SerializeField] public Button ShowDailyRewardsButton;
    [SerializeField] public Button ShowWeeklyRewardsButton;
    [SerializeField] public Slider DailyRewardTimerSlider;
    [SerializeField] public Slider WeeklyRewardTimerSlider;


    public int CurrentActiveDailySlot
    {
        get => PlayerPrefs.GetInt(RewardDataKeys.ActiveDailySlotKey);
        set => PlayerPrefs.SetInt(RewardDataKeys.ActiveDailySlotKey, value);        
    }
    public int CurrentActiveWeeklySlot
    {
        get => PlayerPrefs.GetInt(RewardDataKeys.ActiveWeeklySlotKey);
        set => PlayerPrefs.SetInt(RewardDataKeys.ActiveWeeklySlotKey, value);
    }

    public DateTime? LastDailyRewardTime
    {
        get
        {
            var data = PlayerPrefs.GetString(RewardDataKeys.DailyTimeKey);
            if (string.IsNullOrEmpty(data))
                return null;
            return DateTime.Parse(data);
        }
        set
        {
            if (value != null)
                PlayerPrefs.SetString(RewardDataKeys.DailyTimeKey, value.ToString());
            else
                PlayerPrefs.DeleteKey(RewardDataKeys.DailyTimeKey);
        }
    }
    public DateTime? LastWeeklyRewardTime
    {
        get
        {
            var data = PlayerPrefs.GetString(RewardDataKeys.WeeklyTimeKey);
            if (string.IsNullOrEmpty(data))
                return null;
            return DateTime.Parse(data);
        }
        set
        {
            if (value != null)
                PlayerPrefs.SetString(RewardDataKeys.WeeklyTimeKey, value.ToString());
            else
                PlayerPrefs.DeleteKey(RewardDataKeys.WeeklyTimeKey);
        }
    }


    public void ResetRewardsShowCondition()
    {
        DailySlotsParent.gameObject.SetActive(false);
        WeeklySlotsParent.gameObject.SetActive(false);
        ShowDailyRewardsButton.image.color = Color.white;
        ShowWeeklyRewardsButton.image.color = Color.white;
    }

    private void OnDestroy()
    {
        GetRewardButton.onClick.RemoveAllListeners();
        ResetButton.onClick.RemoveAllListeners();
        ShowDailyRewardsButton.onClick.RemoveAllListeners();
        ShowWeeklyRewardsButton.onClick.RemoveAllListeners();
    }
    }
}