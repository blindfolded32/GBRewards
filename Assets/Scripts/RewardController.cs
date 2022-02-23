﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class RewardController
    {
        private readonly RewardView _rewardView;
    private List<SlotRewardView> _daylySlots;
    private List<SlotRewardView> _weeklySlots;
    private SaveRepository _saveRepository;

    private bool _dailyRewardReceived = false;
    private bool _weeklyRewardReceived = false;
    public RewardController(RewardView rewardView, SaveRepository saveRepository)
    {
        _rewardView = rewardView;
        _saveRepository = saveRepository;

        saveRepository.Load();
        InitSlots();
        RefreshUi();
        _rewardView.StartCoroutine(UpdateCoroutine());
        SubscribeButtons();

        _rewardView.ShowDailyRewardsButton.onClick.Invoke();
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            Update();
            yield return new WaitForSeconds(1);
        }
    }

    private void Update()
    {
        RefreshRewardState();
        RefreshUi();
        _saveRepository.Save();
    }

    private void RefreshRewardState()
    {
   //     _dailyRewardReceived = false;
    //    _weeklyRewardReceived = false;

        if (_rewardView.LastDailyRewardTime.HasValue)
        {
            _dailyRewardReceived = RefreshSlots(  _rewardView.DayTimeDeadline,_rewardView.LastDailyRewardTime,
                RewardDataKeys.DaylyTimerKey, _rewardView.DayTimeCooldown);          
        }

        if (_rewardView.LastWeeklyRewardTime.HasValue)
        {
            _weeklyRewardReceived = RefreshSlots(  _rewardView.WeekTimeDeadline,  _rewardView.LastWeeklyRewardTime,
                RewardDataKeys.WeeklyTimerKey,_rewardView.WeekTimeCooldown);
        }

        bool RefreshSlots( int timeDeadLine, DateTime? lastRewardTime, string rewardType, int timeCD)
        {
            var timeSpan = DateTime.UtcNow - lastRewardTime.Value;

            if (timeSpan.Seconds > timeDeadLine)
            {
                if (rewardType == RewardDataKeys.WeeklyTimerKey)
                {
                    _rewardView.LastDailyRewardTime = null;
                }

                if (rewardType == RewardDataKeys.DaylyTimerKey)
                {
                    _rewardView.LastWeeklyRewardTime = null;
                }
               
                _rewardView.CurrentActiveDailySlot = 0;
            }
            else if (timeSpan.Seconds < timeCD)
            {
                return true;
            }
            return false;
        }
    }

    private void RefreshUi()
    {
        if (_rewardView.DailySlotsParent.gameObject.activeInHierarchy)
        {
            _rewardView.GetRewardButton.interactable = !_dailyRewardReceived;
        }

        if (_rewardView.WeeklySlotsParent.gameObject.activeInHierarchy)
        {
            _rewardView.GetRewardButton.interactable = !_weeklyRewardReceived;
        }

        for (var i = 0; i < _rewardView.DailyRewards.Count-1; i++)
        {
            _daylySlots[i].SetData(i <= _rewardView.CurrentActiveDailySlot);
        }

        for (var i = 0; i < _rewardView.WeeklyRewards.Count-1; i++)
        {
            _weeklySlots[i].SetData(i <= _rewardView.CurrentActiveWeeklySlot);
        }

        DateTime nextDailyBonusTime = !_rewardView.LastDailyRewardTime.HasValue ? DateTime.MinValue
                : _rewardView.LastDailyRewardTime.Value.AddSeconds(_rewardView.DayTimeCooldown);

        var dayDelta = nextDailyBonusTime - DateTime.UtcNow;
        if (dayDelta.TotalSeconds < 0)
            dayDelta = new TimeSpan(0);

        _rewardView.DailyRewardTimer.text = dayDelta.ToString();


        _rewardView.DailyRewardTimerSlider.value = 
                    (_rewardView.DayTimeCooldown - (float)dayDelta.TotalSeconds) / _rewardView.DayTimeCooldown;

        DateTime nextWeeklyBonusTime = !_rewardView.LastWeeklyRewardTime.HasValue ? DateTime.MinValue
                : _rewardView.LastWeeklyRewardTime.Value.AddSeconds(_rewardView.WeekTimeCooldown);

        var weekDelta = nextWeeklyBonusTime - DateTime.UtcNow;
        if (weekDelta.TotalSeconds < 0)
            weekDelta = new TimeSpan(0);

        _rewardView.WeeklyRewardTimer.text = weekDelta.ToString();
        _rewardView.WeeklyRewardTimerSlider.value =
                    (_rewardView.WeekTimeCooldown - (float)weekDelta.TotalSeconds) / _rewardView.WeekTimeCooldown;
    }

    private void InitSlots()
    {
        _daylySlots = new List<SlotRewardView>();
        _weeklySlots = new List<SlotRewardView>();

        for (int i = 0; i < _rewardView.DailyRewards.Count; i++)
        {
            var reward = _rewardView.DailyRewards[i];
            var slotInstance = GameObject.Instantiate(_rewardView.SlotPrefab, _rewardView.DailySlotsParent, false);
            slotInstance.SetData(reward, RewardDataKeys.DaylyTimerKey, i + 1, false);
            _daylySlots.Add(slotInstance);
        }
        for (int i = 0; i < _rewardView.WeeklyRewards.Count; i++)
        {
            var reward = _rewardView.WeeklyRewards[i];
            var slotInstance = GameObject.Instantiate(_rewardView.SlotPrefab, _rewardView.WeeklySlotsParent, false);
            slotInstance.SetData(reward, RewardDataKeys.WeeklyTimerKey, i + 1, false);
            _weeklySlots.Add(slotInstance);
        }
    }

    private void SubscribeButtons()
    {
        _rewardView.GetRewardButton.onClick.AddListener(ClaimReward);
        _rewardView.ResetButton.onClick.AddListener(ResetReward);
        _rewardView.ShowDailyRewardsButton.onClick.AddListener(() => 
                    SetRewardWindow(_rewardView.DailySlotsParent, _rewardView.ShowDailyRewardsButton));
        _rewardView.ShowWeeklyRewardsButton.onClick.AddListener(() => 
                    SetRewardWindow(_rewardView.WeeklySlotsParent, _rewardView.ShowWeeklyRewardsButton));
    }

    private void SetRewardWindow(Transform grid, Button button)
    {
        if (button.image.color == Color.green)
            return;

        _rewardView.ResetRewardsShowCondition();
        button.image.color = Color.green;
        grid.gameObject.SetActive(true);
        RefreshUi();
    }

    private void ResetReward()
    {
        _rewardView.LastDailyRewardTime = null;
        _rewardView.CurrentActiveDailySlot = 0;
        _rewardView.LastWeeklyRewardTime = null;
        _rewardView.CurrentActiveWeeklySlot = 0;
    }

    private void ClaimReward()
    {
        if (_rewardView.DailySlotsParent.gameObject.activeInHierarchy)
        {
            if (_dailyRewardReceived)
                return;

            var reward = _rewardView.DailyRewards[_rewardView.CurrentActiveDailySlot];
            switch (reward.Type)
            {
                case RewardType.None:
                    break;
                case RewardType.Wood:
                    CurrencyWindow.Instance.AddWood(reward.Count);
                    break;
                case RewardType.Diamond:
                    CurrencyWindow.Instance.AddDiamond(reward.Count);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _rewardView.LastDailyRewardTime = DateTime.UtcNow;
            _rewardView.CurrentActiveDailySlot = (_rewardView.CurrentActiveDailySlot + 1) % _rewardView.DailyRewards.Count;
        }

        if (_rewardView.WeeklySlotsParent.gameObject.activeInHierarchy)
        {
            if (_weeklyRewardReceived)
                return;

            var reward = _rewardView.WeeklyRewards[_rewardView.CurrentActiveWeeklySlot];
            switch (reward.Type)
            {
                case RewardType.None:
                    break;
                case RewardType.Wood:
                    CurrencyWindow.Instance.AddWood(reward.Count);
                    break;
                case RewardType.Diamond:
                    CurrencyWindow.Instance.AddDiamond(reward.Count);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _rewardView.LastWeeklyRewardTime = DateTime.UtcNow;
            _rewardView.CurrentActiveWeeklySlot = (_rewardView.CurrentActiveWeeklySlot + 1) % _rewardView.WeeklyRewards.Count;
        }       

        RefreshRewardState();
    }
    }
}