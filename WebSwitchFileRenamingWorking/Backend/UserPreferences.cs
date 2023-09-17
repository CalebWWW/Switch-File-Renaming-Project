namespace WebSwitchFileRenamingWorking.Backend
{
    public class UserPreferences
    {
        public static bool ReplaceUi { get; set; }
        public static bool ReplaceFighter { get; set; }
        public static bool ReplaceJson { get; set; }
        public static string FileToKeep { get; set; }
        public static string LocationToMove { get; set; }

    }
}
