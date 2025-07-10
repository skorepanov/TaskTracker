namespace TaskTracker.Bll;

public static class ErrorMessages
{
    #region UserTask
    public const string UserTaskTitleIsEmpty = "Не заполнено название задачи";

    public static string UserTaskNotFound(int taskId)
        => $"Задача не обнаружена: id = {taskId}";

    public static string UserTaskIsInTrashAlready(int taskId)
        => $"Задача уже находится в корзине: id = {taskId}";

    public static string UserTaskIsNotInTrash(int taskId)
        => $"Задача не находится в корзине: id = {taskId}";

    public static string CantDeleteUserTaskThatIsNotInTrash(int taskId)
        => $"Невозможно удалить задачу не из корзины: id = {taskId}";
    #endregion

    #region Folder
    public const string FolderTitleIsEmpty = "Не заполнено название папки";

    public static string FolderNotFound(int folderId)
        => $"Папка не обнаружена: id = {folderId}";
    #endregion

    #region Tag
    public const string TagTitleIsEmpty = "Не заполнено название тега";
    public const string TagColorIsEmpty = "Не указан цвет тега";

    public static string TagNotFound(int tagId)
        => $"Тег не обнаружен: id = {tagId}";

    public static string TagsNotFound(IList<int> tagIds)
    {
        var tagIdsAsString = string.Join(", ", tagIds);
        return $"Теги не обнаружены: ids = {tagIdsAsString}";
    }
    #endregion
}
