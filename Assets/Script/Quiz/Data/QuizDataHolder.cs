// This script holds the data for the quiz application, including the username,
// selected topic, difficulty, and user type. It is used to pass data between different parts of the quiz system.
// exist across multiple scenes.

public static class QuizDataHolder
{
    public static string Username;
    public static QuizEnum.QuizUserGender Gender;
    public static int Age;
    public static QuizTopicSO selectedTopic;
    public static QuizEnum.QuizDifficulty selectedDifficulty;
    public static QuizEnum.QuizUserType selectedUserType;
}