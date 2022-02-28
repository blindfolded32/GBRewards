using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class RewardSaveModel
    {
        public List<IntPrefsData> IntPrefsData;
        public List<StringPrefsData> StringPrefsData;
        public RewardSaveModel()
        {
            IntPrefsData = new List<IntPrefsData>(RewardDataKeys.IntKeys.Count);

            for (int i = 0; i < RewardDataKeys.IntKeys.Count; i++)
            {
                if (PlayerPrefs.HasKey(RewardDataKeys.IntKeys[i]))
                {
                    var key = RewardDataKeys.IntKeys[i];
                    IntPrefsData.Add(new IntPrefsData(key, PlayerPrefs.GetInt(key)));
                }
            }

            StringPrefsData = new List<StringPrefsData>(RewardDataKeys.StringKeys.Count);

            for (int i = 0; i < RewardDataKeys.StringKeys.Count; i++)
            {
                if (PlayerPrefs.HasKey(RewardDataKeys.StringKeys[i]))
                {
                    var key = RewardDataKeys.StringKeys[i];
                    StringPrefsData.Add(new StringPrefsData(key, PlayerPrefs.GetString(key)));
                }
            }
        }

        public void InitSaveData()
        {
            for (int i = 0; i < IntPrefsData.Count; i++)
            {
                PlayerPrefs.SetInt(IntPrefsData[i].Key, IntPrefsData[i].Value);
            }

            for (int i = 0; i < StringPrefsData.Count; i++)
            {
                PlayerPrefs.SetString(StringPrefsData[i].Key, StringPrefsData[i].Value);
            }
        }
    }
}