namespace roider.Models;

public enum LessonStatus
{
    NotStarted = 0,
    InProgress = 1,
    Completed = 2
}

public static class LessonStatusExtensions
{
    public static string ToStatusString(this LessonStatus status)
    {
        return status switch
        {
            LessonStatus.NotStarted => "Not Started",
            LessonStatus.InProgress => "In Progress",
            LessonStatus.Completed => "Completed",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }

    public static LessonStatus FromStatusString(string statusString)
    {
        return statusString switch
        {
            "Not Started" => LessonStatus.NotStarted,
            "In Progress" => LessonStatus.InProgress,
            "Completed" => LessonStatus.Completed,
            _ => throw new ArgumentOutOfRangeException(nameof(statusString), statusString, null)
        };
    }
}