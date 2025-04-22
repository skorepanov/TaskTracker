export default interface ITask {
    id: number | null,
    title: string,
    description: string,
    folderId: number | null,
    completionDate: Date | null,
    dueDateTime: Date | null,
}
