export default interface ITask {
    id: number | null,
    title: string,
    description: string,
    completionDate: Date | null,
    dueDate: Date | null,
    folderId: number,
}
