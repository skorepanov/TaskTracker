namespace TaskTracker.Bll.Models;

/// <summary>
/// Данные для перемещения задачи из корзины
/// </summary>
public record UserTaskForMoveFromTrashDto(
    int? FolderId,
    DateTime? ModifiedDateTime);