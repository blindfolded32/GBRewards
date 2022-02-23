using System.Collections.Generic;

namespace DefaultNamespace
{
    public class RewardDataKeys
    {
        public static readonly string DailyTimeKey = "DailyRewardTime";
        public static readonly string WeeklyTimeKey = "WeeklyRewardTime";
        public static readonly string ActiveDailySlotKey = "ActiveDailySlot";
        public static readonly string ActiveWeeklySlotKey = "ActiveWeeklySlot";

        public static readonly string DaylyTimerKey = "Day";
        public static readonly string WeeklyTimerKey = "Week";

        public static readonly string WoodKey = "Wood";
        public static readonly string DiamondKey = "Diamond";

        public static readonly List<string> IntKeys;
        public static readonly List<string> StringKeys;
        static RewardDataKeys()
        {
            IntKeys = new List<string>()
            {           
                ActiveDailySlotKey,
                ActiveWeeklySlotKey,
                WoodKey,
                DiamondKey
            };
            StringKeys = new List<string>()
            {
                DailyTimeKey,
                WeeklyTimeKey,            
            };
        }       
    }
}