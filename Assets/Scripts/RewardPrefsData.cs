using System;

namespace DefaultNamespace
{
    [Serializable]
    public class IntPrefsData
    {
        public string Key;
        public int Value;

        public IntPrefsData(string key, int value)
        {
            Key = key;
            Value = value;
        }
    }
    [Serializable]
    public class StringPrefsData
    {
        public string Key;
        public string Value;

        public StringPrefsData(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}