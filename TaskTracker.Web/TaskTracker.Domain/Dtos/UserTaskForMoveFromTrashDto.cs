namespace TaskTracker.Domain.Dtos;

/// <summary>
/// Данные для перемещения задачи из корзины
/// </summary>
public record UserTaskForMoveFromTrashDto(
    int? FolderId,
    DateTime? ModifiedDateTime);