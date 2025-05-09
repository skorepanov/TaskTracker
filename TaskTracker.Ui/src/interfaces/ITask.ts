export default interface ITask {
    id: number,
    title: string,
    description: string | null,
    folderId: number | null,
    completedDateTime: Date | null,
    dueDateTime: Date | null,
    movedToTrashDateTime: Date | null
}
