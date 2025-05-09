export default interface ITask {
    id: number | null,
    title: string,
    description: string,
    folderId: number | null,
    completedDateTime: Date | null,
    dueDateTime: Date | null,
    movedToTrashDateTime: Date
}
