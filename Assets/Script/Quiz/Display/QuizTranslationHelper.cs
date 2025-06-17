using UnityEngine;

public static class QuizTranslationHelper
{
    // UserType
    public static string UserTypeToThai(string userType)
    {
        switch (userType)
        {
            case "Normal": return "ผู้ใช้ทั่วไป";
            case "Cripple": return "ผู้บกพร่องทางการได้ยิน";
            default: return userType;
        }
    }
    public static string UserTypeToEnglish(string userType)
    {
        switch (userType)
        {
            case "ผู้ใช้ทั่วไป": return "Normal";
            case "ผู้บกพร่องทางการได้ยิน": return "Cripple";
            default: return userType;
        }
    }

    // Difficulty
    public static string DifficultyToThai(string difficulty)
    {
        switch (difficulty)
        {
            case "Easy": return "ง่าย";
            case "Hard": return "ยาก";
            default: return difficulty;
        }
    }
    public static string DifficultyToEnglish(string difficulty)
    {
        switch (difficulty)
        {
            case "ง่าย": return "Easy";
            case "ยาก": return "Hard";
            default: return difficulty;
        }
    }

    // Gender
    public static string GenderToThai(string gender)
    {
        switch (gender)
        {
            case "Male": return "เพศชาย";
            case "Female": return "เพศหญิง";
            case "LGBTQ": return "เพศทางเลือก";
            default: return "ไม่ระบุ";
        }
    }
    public static string GenderToEnglish(string gender)
    {
        switch (gender)
        {
            case "เพศชาย": return "Male";
            case "เพศหญิง": return "Female";
            case "เพศทางเลือก": return "LGBTQ";
            default: return "Unknown";
        }
    }

    // App Version
    public static string VersionToThai(string version)
    {
        return $"เวอร์ชั่น {version}";
    }
    public static string VersionToEnglish(string version)
    {
        // Remove "เวอร์ชั่น " prefix if present
        if (version.StartsWith("เวอร์ชั่น "))
            return version.Replace("เวอร์ชั่น ", "");
        return version;
    }

    // Correctness
    public static string WasCorrectToThai(string wasCorrect)
    {
        if (wasCorrect == "True" || wasCorrect == "ถูกต้อง") return "ถูกต้อง";
        return "ผิด";
    }
    public static string WasCorrectToEnglish(string wasCorrect)
    {
        if (wasCorrect == "ถูกต้อง" || wasCorrect == "True") return "True";
        return "False";
    }

    // Time formatting
    public static string TimeToThai(string secondsStr)
    {
        if (float.TryParse(secondsStr, out float seconds))
        {
            if (seconds >= 60f)
            {
                int minutes = (int)(seconds / 60f);
                int remainingSeconds = (int)(seconds % 60f);
                return $"{minutes} นาที {remainingSeconds} วินาที";
            }
            else
            {
                return $"{seconds:F2} วินาที";
            }
        }
        return secondsStr;
    }
    public static string TimeToEnglish(string timeStr)
    {
        // Try to parse "X นาที Y วินาที" or "Z วินาที"
        if (timeStr.Contains("นาที"))
        {
            var parts = timeStr.Split(new[] { "นาที", "วินาที" }, System.StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2 &&
                int.TryParse(parts[0].Trim(), out int minutes) &&
                int.TryParse(parts[1].Trim(), out int seconds))
            {
                return (minutes * 60 + seconds).ToString();
            }
        }
        else if (timeStr.Contains("วินาที"))
        {
            var num = timeStr.Replace("วินาที", "").Trim();
            return num;
        }
        return timeStr;
    }
    public static string SortByThaiToEnglish(string sortByThai)
    {
        switch (sortByThai)
        {
            case "คะแนน": return "Score";
            case "เวลา": return "Time";
            default: return sortByThai;
        }
    }
}
